namespace BanterBot.NET.Music
{
    public class TrackArgs
    {
        public ulong Id => Queue.Id;

        public MusicQueue Queue { get; set; }

        public IMusicTrack Track { get; set; }

        public TrackArgs(MusicQueue queue, IMusicTrack track)
        {
            Queue = queue;
            Track = track;
        }
    }
}
