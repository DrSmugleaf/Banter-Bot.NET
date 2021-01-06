using System.Threading.Tasks;
using BanterBot.NET.Dependencies;
using Victoria;
using Victoria.EventArgs;

namespace BanterBot.NET.Music
{
    [Service]
    public class MusicService
    {
        public LavaNode LavaNode { get; }

        public MusicService(LavaNode lavaNode)
        {
            LavaNode = lavaNode;
            LavaNode.OnTrackEnded += TrackEnded;
        }

        public async Task TrackEnded(TrackEndedEventArgs args)
        {
            if (!args.Reason.ShouldPlayNext() ||
                !args.Player.Queue.TryDequeue(out var track))
            {
                return;
            }

            await args.Player.PlayAsync(track);
            await args.Player.TextChannel.SendMessageAsync($"Now playing `{track.Title}`");
        }
    }
}
