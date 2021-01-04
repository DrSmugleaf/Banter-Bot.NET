using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using BanterBot.NET.Dependencies;
using BanterBot.NET.Extensions;
using BanterBot.NET.Logging;
using BanterBot.NET.Music;
using Discord;
using Discord.Audio;
using JetBrains.Annotations;
using YoutubeExplode;

namespace BanterBot.NET.Commands.Music
{
    [Service]
    public class DiscordMusicService : IService, IDisposable
    {
        [ServiceDependency] public YoutubeClient YoutubeClient { get; set; } = default!;
        [ServiceDependency] public MusicService MusicService { get; set; } = default!;

        private readonly Dictionary<IMusicTrack, DiscordTrackInformation> _tracks = new();

        public DiscordMusicService()
        {
            Logger.DebugS($"Created {nameof(DiscordMusicService)}");
        }

        public bool CreateQueue(IGuild guild)
        {
            return MusicService.CreateQueue(guild.Id);
        }

        [Pure]
        public MusicQueue? GetQueue(IGuild guild)
        {
            return MusicService.GetQueue(guild.Id);
        }

        [Pure]
        public bool TryGetQueue(IGuild guild, [NotNullWhen(true)] out MusicQueue? queue)
        {
            return MusicService.TryGetQueue(guild.Id, out queue);
        }

        public MusicQueue EnsureQueue(IGuild guild)
        {
            return MusicService.EnsureQueue(guild.Id);
        }

        public bool RemoveQueue(IGuild guild)
        {
            return MusicService.RemoveQueue(guild.Id);
        }

        public bool AddTrack(IMusicTrack track, DiscordTrackInformation information)
        {
            if (_tracks.ContainsKey(track))
            {
                return false;
            }

            _tracks[track] = information;

            var queue = EnsureQueue(information.Channel.Guild);
            queue.AddTrack(track);

            Logger.DebugS($"Added track. Tracks in queue: {queue.Tracks.Count}");

            if (queue.Tracks.Count == 1)
            {
                track.Play();
            }
            else
            {
                var title = information.Title;
                var author = information.Author;

                information.Channel.SendMessageAsync($"Added {title} by {author} to the queue.");
            }

            return true;
        }

        public void AfterInject()
        {
            MusicService.TrackStarted += TrackStarted;
            MusicService.TrackStopped += TrackStopped;
        }

        private void TrackStarted(TrackArgs args)
        {
            if (!_tracks.TryGetValue(args.Track, out var information))
            {
                return;
            }

            var title = information.Title;
            var author = information.Author;
            var duration = information.Duration;

            information.Channel.SendMessageAsync($"Now playing {title} by {author}. Duration: {duration.TotalSeconds} seconds");
        }

        private void TrackStopped(TrackArgs args)
        {
            Logger.DebugS("Track stopped");
            _tracks.Remove(args.Track);
        }

        private Process CreateStream(string path)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });

            if (process == null)
            {
                throw new InvalidOperationException();
            }

            return process;
        }

        public async Task PlayYoutube(YoutubeTrack track)
        {
            if (!_tracks.TryGetValue(track, out var information))
            {
                return;
            }

            var voiceChannel = information.Submitter.VoiceChannel;

            if (voiceChannel == null)
            {
                return;
            }

            var client = await voiceChannel.EnsureConnectAsync();
            using var ffmpeg = CreateStream(track.StreamInfo.Url);
            await using var output = ffmpeg.StandardOutput.BaseStream;
            await using var discord = client.CreatePCMStream(AudioApplication.Mixed);

            try
            {
                await output.CopyToAsync(discord);
            }
            finally
            {
                await discord.FlushAsync();
            }
        }

        public void Dispose()
        {
            Logger.DebugS($"Disposing {nameof(DiscordMusicService)}");
            MusicService.TrackStopped -= TrackStopped;
            _tracks.Clear();
        }
    }
}
