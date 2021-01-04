using System.Threading.Tasks;
using BanterBot.NET.Logging;
using Discord;
using Discord.Audio;

namespace BanterBot.NET.Extensions
{
    public static class VoiceChannelExtensions
    {
        public static async Task<IAudioClient?> EnsureConnectAsync(this IVoiceChannel channel)
        {
            DebugTools.AssertNotNull(channel);

            var user = await channel.Guild.GetCurrentUserAsync();

            if (user.Guild.AudioClient == null ||
                user.VoiceChannel != channel)
            {
                Logger.DebugS($"Connecting to channel {channel.Name}");
                return await channel.ConnectAsync();
            }

            Logger.DebugS($"Using existing connection for channel {channel.Name}");
            return user.Guild.AudioClient;
        }
    }
}
