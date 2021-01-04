using System;

namespace BanterBot.NET.Music
{
    public class TrackStoppedArgs : EventArgs
    {
        public ulong Id { get; }

        public MusicQueue Queue { get; }

        public IMusicTrack Track { get; }

        public TrackStoppedArgs(ulong id, MusicQueue queue, IMusicTrack track)
        {
            Id = id;
            Queue = queue;
            Track = track;
        }
    }
}
