using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BanterBot.NET.Dependencies;
using Victoria;
using Victoria.EventArgs;

namespace BanterBot.NET.Music
{
    [Service]
    public class MusicService : IDisposable
    {
        public LavaNode LavaNode { get; }

        private Process LavaLink { get; }

        public MusicService(LavaNode lavaNode)
        {
            LavaNode = lavaNode;
            LavaNode.OnTrackEnded += TrackEnded;

            LavaLink = Process.Start(new ProcessStartInfo
            {
                FileName = "java",
                Arguments = "-jar Lavalink.jar",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }) ?? throw new InvalidOperationException();

            LavaLink.OutputDataReceived += OutputDataReceived;
            LavaLink.ErrorDataReceived += ErrorDataReceived;
            LavaLink.BeginOutputReadLine();
            LavaLink.BeginErrorReadLine();
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
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

        public void Dispose()
        {
            LavaLink.Dispose();
        }
    }
}
