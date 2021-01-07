using System;
using System.Threading.Tasks;
using BanterBot.NET.Extensions;
using BanterBot.NET.Music;
using Discord;
using Discord.Commands;
using Victoria;
using Victoria.Enums;

namespace BanterBot.NET.Commands.Music
{
    public class PlayCommand : Module<SocketCommandContext>
    {
        public LavaNode LavaNode { get; set; } = default!;

        [Command("play", RunMode = RunMode.Async)]
        public async Task Play(params string[] args)
        {
            if (User is not IGuildUser user || Channel is not ITextChannel text)
            {
                await ReplyAsync("This command can only be used in a server.");
                return;
            }

            if (user.VoiceChannel == null)
            {
                await ReplyAsync("You must be in a voice channel to use this command.");
                return;
            }

            var query = string.Join(" ", args);
            var search = Uri.IsWellFormedUriString(query, UriKind.Absolute) ?
                await LavaNode.SearchAsync(query) :
                await LavaNode.SearchYouTubeAsync(query);

            switch (search.LoadStatus)
            {
                case LoadStatus.TrackLoaded:
                case LoadStatus.PlaylistLoaded:
                case LoadStatus.SearchResult:
                    var player = await LavaNode.EnsureJoin(user.VoiceChannel, text);

                    if (!search.Tracks.TryFirst(out var first))
                    {
                        await ReplyAsync($"No matches found for `{query}`");
                        return;
                    }

                    var playing = await player.PlayOrEnqueue(first);
                    var response = playing
                        ? $"Now playing `{first.Title}`"
                        : $"Added `{first.Title}` to the queue.";

                    await ReplyAsync(response);
                    break;
                case LoadStatus.NoMatches:
                    await ReplyAsync($"No matches found for `{query}`");
                    return;
                case LoadStatus.LoadFailed:
                    await ReplyAsync($"An error occurred when searching for `{query}`");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
