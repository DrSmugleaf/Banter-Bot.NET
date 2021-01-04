using System.Threading.Tasks;
using BanterBot.NET.Logging;
using BanterBot.NET.Music;
using YoutubeExplode.Videos.Streams;
using static BanterBot.NET.Music.IMusicTrack;

namespace BanterBot.NET.Commands.Music
{
    public class YoutubeTrack : IMusicTrack
    {
        public delegate Task PlayDelegate(YoutubeTrack track);

        private PlayDelegate PlayMethod { get; }

        public IStreamInfo StreamInfo { get; }

        public event TrackEventHandler? Started;

        public event TrackEventHandler? Stopped;

        public YoutubeTrack(IStreamInfo info, PlayDelegate play)
        {
            StreamInfo = info;
            PlayMethod = play;
        }

        public async void Play()
        {
            Logger.DebugS("Playing track");
            Started?.Invoke(this);
            await PlayMethod.Invoke(this);
            Stopped?.Invoke(this);
        }
    }
}
