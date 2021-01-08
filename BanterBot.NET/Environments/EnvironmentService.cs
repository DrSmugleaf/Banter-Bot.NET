using System;
using System.IO;
using BanterBot.NET.Dependencies;
using BanterBot.NET.Logging;

namespace BanterBot.NET.Environments
{
    [Service]
    public class EnvironmentService
    {
        public EnvironmentService()
        {
            var directory = Environment.CurrentDirectory;
            var envFilePath = directory + "/.env";

            if (File.Exists(envFilePath))
            {
                var lines = File.ReadLines(envFilePath);

                foreach (var line in lines)
                {
                    var split = line.Split('=', 2);
                    var key = split[0];
                    var value = split[1];

                    if (Environment.GetEnvironmentVariable(key) != null)
                    {
                        Logger.WarningS($"Replacing existing environment variable {key}");
                    }

                    Environment.SetEnvironmentVariable(key, value);
                }
            }
        }
    }
}
