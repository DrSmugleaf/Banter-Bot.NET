namespace BanterBot.NET.Music
{
    public interface IMusicTrack
    {
        delegate void TrackEventHandler(IMusicTrack track);

        event TrackEventHandler? Started;

        event TrackEventHandler? Stopped;

        public void Play();
    }
}
