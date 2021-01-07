using System;
using System.IO;
using BanterBot.NET.Dependencies;

namespace BanterBot.NET.Environments
{
    [Service]
    public class EnvironmentService
    {
        public EnvironmentService()
        {
            var directory = Environment.CurrentDirectory;
            var lines = File.ReadLines(directory + "/.env");

            foreach (var line in lines)
            {
                var split = line.Split('=', 2);
                var key = split[0];
                var value = split[1];

                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}
