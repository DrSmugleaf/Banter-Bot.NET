using System;
using Discord;

namespace BanterBot.NET.Logging
{
    public static class SeverityExtensions
    {
        public static Severity From(LogSeverity severity)
        {
            return severity switch
            {
                LogSeverity.Critical => Severity.Fatal,
                LogSeverity.Error => Severity.Error,
                LogSeverity.Warning => Severity.Warning,
                LogSeverity.Info => Severity.Info,
                LogSeverity.Verbose => Severity.Verbose,
                LogSeverity.Debug => Severity.Debug,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static LogSeverity ToDiscord(Severity severity)
        {
            return severity switch
            {
                Severity.Fatal => LogSeverity.Critical,
                Severity.Error => LogSeverity.Error,
                Severity.Warning => LogSeverity.Warning,
                Severity.Info => LogSeverity.Info,
                Severity.Verbose => LogSeverity.Verbose,
                Severity.Debug => LogSeverity.Debug,
                _ => throw new ArgumentOutOfRangeException()
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
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
