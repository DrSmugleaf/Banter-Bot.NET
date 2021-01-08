using System.Linq;
using System.Threading.Tasks;
using BanterBot.NET.Database.Guilds;
using Discord;
using Discord.Commands;

namespace BanterBot.NET.Commands
{
    public class PrefixCommand : Module<SocketCommandContext>
    {
        [Command("prefix")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Prefix(params string[] args)
        {
            if (Guild == null)
            {
                await ReplyAsync("This command can only be used in a server channel.");
                return;
            }

            await using var guildContext = new GuildContext();
            var guild = guildContext.Guilds.Single(g => g.Id == Guild.Id);
            var prefix = string.Join(" ", args);

            if (string.IsNullOrWhiteSpace(prefix))
            {
                guild.Prefix = null;
                await guildContext.SaveChangesAsync();
                await ReplyAsync("Command prefix reset to the default, `!`");
                return;
            }

            guild.Prefix = prefix;
            await guildContext.SaveChangesAsync();
            await ReplyAsync($"Command prefix changed to `{prefix}`");
        }
    }
}
