using System;

namespace BanterBot.NET.Logging
{
    public class Logger
    {
        private static readonly Logger Default = new Logger("Default");

        public Logger(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static void LogS(Severity severity, string message)
        {
            Default.Log(severity, message);
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

        public void Log(Severity severity, string message)
        {
            Console.WriteLine($"[{severity.Abbreviation()}] {Name}: {message}");
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
