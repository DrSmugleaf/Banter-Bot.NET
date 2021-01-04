using System;
using System.Threading.Tasks;
using Discord;

namespace BanterBot.NET.Logging
{
    public class Logger
    {
        private static readonly Logger Default = new("Default");

        public Logger(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static void LogS(Severity severity, string message)
        {
            Default.Log(severity, message);
        }

        public static Task LogS(LogMessage message)
        {
            return Default.Log(message);
        }

        public static void DebugS(string message)
        {
            Default.Debug(message);
        }

        public static void VerboseS(string message)
        {
            Default.Verbose(message);
        }

        public static void InfoS(string message)
        {
            Default.Info(message);
        }

        public static void WarningS(string message)
        {
            Default.Warning(message);
        }

        public static void ErrorS(string message)
        {
            Default.Error(message);
        }

        public static void FatalS(string message)
        {
            Default.Fatal(message);
        }

        public Task Log(Severity severity, string message)
        {
            Console.ForegroundColor = severity.Color();

            Console.WriteLine($"{DateTime.Now,-19} [{severity.Abbreviation()}] {Name}: {message}");

            Console.ResetColor();

            return Task.CompletedTask;
        }

        public Task Log(Severity severity, string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            Console.WriteLine($"{DateTime.Now,-19} [{severity.Abbreviation()}] {Name}: {message}");

            Console.ResetColor();

            return Task.CompletedTask;
        }

        public Task Log(LogMessage message)
        {
            Console.ForegroundColor = message.Severity.ToSeverity().Color();

            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity}] {message.Source}: {message.Message} {message.Exception}");

            Console.ResetColor();

            return Task.CompletedTask;
        }

        public void Debug(string message)
        {
            Log(Severity.Debug, message);
        }

        public void Verbose(string message)
        {
            Log(Severity.Verbose, message);
        }

        public void Info(string message)
        {
            Log(Severity.Info, message);
        }

        public void Warning(string message)
        {
            Log(Severity.Warning, message);
        }

        public void Error(string message)
        {
            Log(Severity.Error, message);
        }

        public void Fatal(string message)
        {
            Log(Severity.Fatal, message);
        }
    }
}
