using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BanterBot.NET.Dependencies;
using JetBrains.Annotations;

namespace BanterBot.NET.Music
{
    [Service]
    public class MusicService : IDisposable
    {
        private readonly Dictionary<ulong, MusicQueue> _queues = new();

        public IReadOnlyDictionary<ulong, MusicQueue> Queues => _queues;

        public delegate void MusicEventHandler(TrackArgs args);

        public event MusicEventHandler? TrackStarted;

        public event MusicEventHandler? TrackStopped;

        private MusicQueue AddQueue(ulong id)
        {
            var queue = new MusicQueue(id);
            queue.TrackStarted += OnTrackStarted;
            queue.TrackStopped += OnTrackStopped;
            _queues[id] = queue;

            return queue;
        }

        public bool CreateQueue(ulong id)
        {
            if (_queues.ContainsKey(id))
            {
                return false;
            }

            AddQueue(id);
            return true;
        }

        [Pure]
        public MusicQueue? GetQueue(ulong id)
        {
            return _queues.GetValueOrDefault(id);
        }

        [Pure]
        public bool TryGetQueue(ulong id, [NotNullWhen(true)] out MusicQueue? queue)
        {
            return _queues.TryGetValue(id, out queue);
        }

        public MusicQueue EnsureQueue(ulong id)
        {
            if (!_queues.TryGetValue(id, out var queue))
            {
                queue = AddQueue(id);
            }

            return queue;
        }

        public bool RemoveQueue(ulong id)
        {
            if (_queues.Remove(id, out var queue))
            {
                queue.TrackStarted -= OnTrackStarted;
                queue.TrackStopped -= OnTrackStopped;
                return true;
            }

            return false;
        }

        private void OnTrackStarted(MusicQueue queue, IMusicTrack track)
        {
            var args = new TrackArgs(queue, track);
            TrackStarted?.Invoke(args);
        }

        private void OnTrackStopped(MusicQueue queue, IMusicTrack track)
        {
            var args = new TrackArgs(queue, track);
            TrackStopped?.Invoke(args);
        }

        public void Dispose()
        {
            foreach (var queue in _queues.Values)
            {
                queue.TrackStarted -= OnTrackStarted;
                queue.TrackStopped -= OnTrackStopped;
            }

            _queues.Clear();
        }
    }
}
