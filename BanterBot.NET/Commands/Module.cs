using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace BanterBot.NET.Commands
{
    public abstract class Module<T> : ModuleBase<T> where T : class, ICommandContext
    {
        protected async Task<IAudioClient> EnsureConnectAsync(IVoiceChannel channel)
        {
            DebugTools.AssertNotNull(channel);

            var user = await channel.Guild.GetCurrentUserAsync();

            if (user.VoiceChannel != channel)
            {
                return await channel.ConnectAsync();
            }

            return user.Guild.AudioClient;
        }
    }
}
