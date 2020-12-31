using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;

namespace BanterBot.NET.Commands
{
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; set; }

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
