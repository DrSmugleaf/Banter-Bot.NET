using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Victoria;
using Victoria.Enums;

namespace BanterBot.NET.Commands.Music
{
    public class SkipCommand : Module<SocketCommandContext>
    {
        public LavaNode LavaNode { get; set; } = default!;

        [Command("skip", RunMode = RunMode.Async)]
        public async Task Skip()
        {
            if (Author is not IGuildUser user)
            {
                await ReplyAsync("This command can only be used in a server.");
                return;
            }
            if (user.VoiceChannel == null)
            {
                await ReplyAsync("You must be in a voice channel to use this command.");
                return;
            }

            if (!LavaNode.TryGetPlayer(user.Guild, out var player))
            {
                await ReplyAsync("No track is currently playing.");
                return;
            }

            switch (player.PlayerState)
            {
                case PlayerState.Connected:
                case PlayerState.Disconnected:
                case PlayerState.Stopped:
                    await ReplyAsync("No track is currently playing.");

                    return;
                case PlayerState.Playing:
                case PlayerState.Paused:
                    if (player.Queue.Count == 0)
                    {
                        await player.StopAsync();
                    }
                    else
                    {
                        await player.SkipAsync();
                    }

                    await ReplyAsync("Track skipped.");

                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
