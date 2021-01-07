using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Victoria;
using Victoria.Enums;

namespace BanterBot.NET.Commands.Music
{
    public class ResumeCommand : Module<SocketCommandContext>
    {
        public LavaNode LavaNode { get; set; } = default!;

        [Command("resume", RunMode = RunMode.Async)]
        public async Task Resume(params string[] args)
        {
            if (User is not IGuildUser user)
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
                    await ReplyAsync("The current track is already playing.");

                    return;
                case PlayerState.Paused:
                    await player.ResumeAsync();
                    await ReplyAsync("Track resumed.");

                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
