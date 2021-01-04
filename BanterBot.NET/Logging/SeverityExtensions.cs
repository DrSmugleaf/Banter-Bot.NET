using System;
using Discord;

namespace BanterBot.NET.Logging
{
    public static class SeverityExtensions
    {
        public static Severity ToSeverity(this LogSeverity severity)
        {
            return severity switch
            {
                LogSeverity.Critical => Severity.Fatal,
                LogSeverity.Error => Severity.Error,
                LogSeverity.Warning => Severity.Warning,
                LogSeverity.Info => Severity.Info,
                LogSeverity.Verbose => Severity.Verbose,
                LogSeverity.Debug => Severity.Debug,
                _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
            };
        }

        public static LogSeverity ToDiscord(this Severity severity)
        {
            return severity switch
            {
                Severity.Fatal => LogSeverity.Critical,
                Severity.Error => LogSeverity.Error,
                Severity.Warning => LogSeverity.Warning,
                Severity.Info => LogSeverity.Info,
                Severity.Verbose => LogSeverity.Verbose,
                Severity.Debug => LogSeverity.Debug,
                _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
            };
        }

        public static string Abbreviation(this Severity severity)
        {
            return severity switch
            {
                Severity.Unknown => "UNKNOWN",
                Severity.Debug => "DEBUG",
                Severity.Verbose => "VERBOSE",
                Severity.Info => "INFO",
                Severity.Warning => "WARNING",
                Severity.Error => "ERROR",
                Severity.Fatal => "FATAL",
                _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
            };
        }

        public static ConsoleColor Color(this Severity severity)
        {
            return severity switch
            {
                Severity.Unknown => ConsoleColor.Gray,
                Severity.Debug => ConsoleColor.Gray,
                Severity.Verbose => ConsoleColor.Gray,
                Severity.Info => ConsoleColor.White,
                Severity.Warning => ConsoleColor.Yellow,
                Severity.Error => ConsoleColor.Red,
                Severity.Fatal => ConsoleColor.Red,
                _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
            };
        }
    }
}
