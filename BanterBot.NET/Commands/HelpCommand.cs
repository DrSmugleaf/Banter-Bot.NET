using System.Collections.Generic;
using System.Threading.Tasks;
using BanterBot.NET.Dependencies;
using Discord.Commands;

namespace BanterBot.NET.Commands
{
    public class HelpCommand : Module<SocketCommandContext>
    {
        [ServiceDependency] public CommandService CommandService { get; } = default!;

        [Command("help")]
        public async Task Help()
        {
            var commands = new List<string>();

            foreach (var command in CommandService.Commands)
            {
                commands.Add(command.Name);
            }

            await ReplyAsync($"Commands: {string.Join(", ", commands)}");
        }
    }
}
