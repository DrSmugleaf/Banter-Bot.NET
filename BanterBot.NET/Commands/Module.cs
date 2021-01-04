using System.Threading.Tasks;
using BanterBot.NET.Extensions;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace BanterBot.NET.Commands
{
    public abstract class Module<T> : ModuleBase<T> where T : class, ICommandContext
    {
        protected IDiscordClient Client => Context.Client;

        protected IGuild Guild => Context.Guild;

        protected IMessageChannel Channel => Context.Channel;

        protected IUser User => Context.User;

        protected IUserMessage Message => Context.Message;
    }
}
