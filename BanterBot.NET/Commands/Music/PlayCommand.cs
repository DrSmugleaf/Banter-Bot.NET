using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace BanterBot.NET.Commands.Music
{
    public class PlayCommand : Module<SocketCommandContext>
    {
        [Command("play", RunMode = RunMode.Async)]
        public async Task Play(params string[] args)
        {
            if (Context.User is not IGuildUser user)
            {
                await ReplyAsync("This command can only be used in a server.");
                return;
            }

            if (user.VoiceChannel == null)
            {
                await ReplyAsync("You must be in a voice channel to use this command.");
                return;
            }

            var client = await EnsureConnectAsync(user.VoiceChannel);
            var youtube = new YoutubeClient();
            var link = string.Join(" ", args);
            var video = await youtube.Videos.GetAsync(link);
            var title = video.Title;
            var author = video.Author;
            var duration = video.Duration;
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();

            if (streamInfo == null)
            {
                await ReplyAsync($"No video found with link {link}");
                return;
            }

            await youtube.Videos.Streams.DownloadAsync(streamInfo, $"video.{streamInfo.Container}");
            await SendAsync(client, streamInfo.Url);

            await ReplyAsync($"Now playing {title} by {author}. Duration: {duration.TotalSeconds} seconds");
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        private async Task SendAsync(IAudioClient client, string path)
        {
            using var ffmpeg = CreateStream(path);
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
    }
}
