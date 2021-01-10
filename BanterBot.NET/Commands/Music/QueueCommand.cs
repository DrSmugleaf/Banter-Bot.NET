using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Victoria;

namespace BanterBot.NET.Commands.Music
{
    public class QueueCommand : Module<SocketCommandContext>
    {
        public LavaNode LavaNode { get; set; } = default!;

        [Command("queue", RunMode = RunMode.Async)]
        public async Task Queue()
        {
            if (Author is not IGuildUser user)
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

            if (player.Track == null &&
                player.Queue.Count == 0)
            {
                await ReplyAsync("No track is currently playing.");
                return;
            }

            var response = new StringBuilder();

            if (player.Track != null)
            {
                response.AppendLine($"Current Track: {player.Track.Title} (Duration: {player.Track.Duration:hh\\:mm\\:ss})");
            }

            foreach (var track in player.Queue)
            {
                response.AppendLine($"{track.Title} (Duration: {track.Duration:hh\\:mm\\:ss})");
            }

            await ReplyAsync(response.ToString());
        }
    }
}
