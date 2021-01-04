using System;
using Discord;

namespace BanterBot.NET.Commands.Music
{
    public class DiscordTrackInformation
    {
        public ITextChannel Channel { get; }

        public IGuildUser Submitter { get; }

        public string Title { get; }

        public string Author { get; }

        public TimeSpan Duration { get; }

        public DiscordTrackInformation(ITextChannel channel, IGuildUser submitter, string title, string author, TimeSpan duration)
        {
            Channel = channel;
            Submitter = submitter;
            Title = title;
            Author = author;
            Duration = duration;
        }
    }
}
