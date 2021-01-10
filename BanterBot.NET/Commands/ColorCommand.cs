using System.Linq;
using System.Threading.Tasks;
using BanterBot.NET.Extensions;
using Discord;
using Discord.Commands;

namespace BanterBot.NET.Commands
{
    public class ColorCommand : Module<SocketCommandContext>
    {
        [Command("color")]
        public async Task Color()
        {
            var guildAuthor = await GuildAuthor;
            var guildClient = await GuildClient;

            if (Guild == null || guildAuthor == null || guildClient == null)
            {
                await ReplyAsync("This command can only be used in a server channel.");
                return;
            }

            var highestAuthorColoredRole = guildAuthor.GetHighestRole(role => role.Color != default);
            var assignedColoredRole = Guild.Roles.FirstOrDefault(role => role.Name == $"color-{guildAuthor.Id}");

            if (assignedColoredRole == null ||
                !guildAuthor.HasRole(assignedColoredRole))
            {
                await ReplyAsync("You don't have a colored role to remove. Use this command with a hex color to add one.");
                return;
            }

            var highestBotRole = guildClient.GetHighestRole();

            if (highestAuthorColoredRole != null &&
                highestAuthorColoredRole.Position > highestBotRole.Position)
            {
                await ReplyAsync($"I don't have permission to edit your highest colored role, `{highestAuthorColoredRole.Name}`. Move my role above it.");
                return;
            }

            if (assignedColoredRole.Position > highestBotRole.Position)
            {
                await ReplyAsync(
                    $"I don't have permission to edit the `{assignedColoredRole.Name}` role. Move my role above it.");
                return;
            }

            await guildAuthor.RemoveRoleAsync(assignedColoredRole);
            await ReplyAsync($"Removed color {assignedColoredRole.Color.ToString()}");
        }

        [Command("color")]
        public async Task Color(string args)
        {
            var guildAuthor = await GuildAuthor;
            var guildClient = await GuildClient;

            if (Guild == null || guildAuthor == null || guildClient == null)
            {
                await ReplyAsync("This command can only be used in a server channel.");
                return;
            }

            var highestAuthorColoredRole = guildAuthor.GetHighestRole(role => role.Color != default);
            var assignedColoredRoleName = $"color-{guildAuthor.Id}";
            var assignedColoredRole = Guild.Roles.FirstOrDefault(role => role.Name == assignedColoredRoleName);
            var highestBotRole = guildClient.GetHighestRole();

            if (highestAuthorColoredRole != null &&
                highestAuthorColoredRole.Position > highestBotRole.Position)
            {
                await ReplyAsync($"I don't have permission to edit your highest colored role, `{highestAuthorColoredRole.Name}`. Move my role above it.");
                return;
            }

            if (assignedColoredRole != null &&
                assignedColoredRole.Position > highestBotRole.Position)
            {
                await ReplyAsync(
                    $"I don't have permission to edit the `{assignedColoredRole.Name}` role. Move my role above it.");
                return;
            }

            var colorString = string.Join(" ", args);

            if (!ColorExtensions.TryParseDiscordColor(colorString, out var color))
            {
                await ReplyAsync($"`{colorString}` is not a valid color.");
                return;
            }

            if (assignedColoredRole == null)
            {
                assignedColoredRole = await Guild.CreateRoleAsync(
                    assignedColoredRoleName,
                    GuildPermissions.None,
                    color,
                    false,
                    false);
                await guildAuthor.AddRoleAsync(assignedColoredRole);
            }
            else
            {
                await assignedColoredRole.ModifyAsync(role => role.Color = color);
            }

            await ReplyAsync($"Added color {assignedColoredRole.Color.ToString()}");
        }
    }
}
