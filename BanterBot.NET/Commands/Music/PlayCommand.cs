using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace BanterBot.NET.Commands.Music
{
    public class PlayCommand : Module<SocketCommandContext>
    {
        public DiscordMusicService DiscordMusicService { get; set; }

        public YoutubeClient YoutubeClient { get; set; }

        [Command("play", RunMode = RunMode.Async)]
        public async Task Play(params string[] args)
        {
            if (User is not IGuildUser user || Channel is not ITextChannel channel)
            {
                await ReplyAsync("This command can only be used in a server.");
                return;
            }

            if (user.VoiceChannel == null)
            {
                await ReplyAsync("You must be in a voice channel to use this command.");
                return;
            }

            var link = string.Join(" ", args);
            var video = await YoutubeClient.Videos.GetAsync(link);
            var title = video.Title;
            var author = video.Author;
            var duration = video.Duration;
            var streamManifest = await YoutubeClient.Videos.Streams.GetManifestAsync(video.Id);
            var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();

            if (streamInfo == null)
            {
                await ReplyAsync($"No video found with link {link}");
                return;
            }

            var youtubeTrack = new YoutubeTrack(streamInfo, DiscordMusicService.PlayYoutube);
            var trackInformation = new DiscordTrackInformation(channel, user, title, author, duration);

            DiscordMusicService.AddTrack(youtubeTrack, trackInformation);
        }
    }
}
