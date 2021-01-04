using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BanterBot.NET.Music
{
    public class MusicQueue
    {
        private readonly List<IMusicTrack> _tracks = new();

        public ulong Id { get; }

        public IReadOnlyList<IMusicTrack> Tracks => _tracks;

        public delegate void QueueEventHandler(MusicQueue queue, IMusicTrack track);

        public event QueueEventHandler? TrackStarted;

        public event QueueEventHandler? TrackStopped;

        public MusicQueue(ulong id)
        {
            Id = id;
        }

        public void AddTrack(IMusicTrack track)
        {
            track.Started += OnTrackStarted;
            track.Stopped += OnTrackStopped;

            _tracks.Add(track);
        }

        public bool RemoveTrack(IMusicTrack track)
        {
            if (_tracks.Remove(track))
            {
                track.Started -= OnTrackStarted;
                track.Stopped -= OnTrackStopped;
                return true;
            }

            return false;
        }

        public bool TrySingle([NotNullWhen(true)] out IMusicTrack? track)
        {
            if (_tracks.Count != 1)
            {
                track = null;
                return false;
            }

            track = _tracks.First();
            return true;
        }

        public bool TryNext([NotNullWhen(true)] out IMusicTrack? track)
        {
            return (track = _tracks.FirstOrDefault()) != default;
        }

        private void OnTrackStarted(IMusicTrack track)
        {
            TrackStarted?.Invoke(this, track);
        }

        private void OnTrackStopped(IMusicTrack track)
        {
            _tracks.Remove(track);
            TrackStopped?.Invoke(this, track);

            if (TryNext(out var nextTrack))
            {
                nextTrack.Play();
            }
        }
    }
}
