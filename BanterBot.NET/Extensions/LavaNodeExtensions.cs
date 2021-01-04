using System;
using System.Threading.Tasks;
using Discord;
using Victoria;
using Victoria.Enums;

namespace BanterBot.NET.Extensions
{
    public static class LavaNodeExtensions
    {
        public static async Task<LavaPlayer> EnsureJoin(this LavaNode node, IVoiceChannel voice, ITextChannel text)
        {
            if (!node.IsConnected)
            {
                throw new InvalidOperationException();
            }

            if (!node.TryGetPlayer(voice.Guild, out var player))
            {
                player = await node.JoinAsync(voice, text);
            }

            return player;
        }

        public static async Task<bool> PlayOrEnqueue(this LavaPlayer player, LavaTrack track)
        {
            if (player.PlayerState == PlayerState.Playing ||
                player.PlayerState == PlayerState.Paused)
            {
                player.Queue.Enqueue(track);
                return false;
            }

            await player.PlayAsync(track);
            return true;
        }
    }
}
