using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace BanterBot.NET.Commands
{
    public abstract class Module<T> : ModuleBase<T> where T : class, ICommandContext
    {
        protected IDiscordClient Client => Context.Client;

        protected Task<IGuildUser?> GuildClient => Guild?.GetCurrentUserAsync() ?? Task.FromResult<IGuildUser?>(null);

        protected IGuild? Guild => Context.Guild;

        protected IMessageChannel Channel => Context.Channel;

        protected IUser Author => Context.User;

        protected Task<IGuildUser?> GuildAuthor => Guild?.GetUserAsync(Author.Id) ?? Task.FromResult<IGuildUser?>(null);

        protected IUserMessage Message => Context.Message;
    }
}
