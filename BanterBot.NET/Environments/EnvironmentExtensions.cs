using System;
using System.Diagnostics.CodeAnalysis;

namespace BanterBot.NET.Environments
{
    public static class EnvironmentExtensions
    {
        public static bool TryGetEnvironmentVariable(string key, [NotNullWhen(true)] out string? value)
        {
            return (value = Environment.GetEnvironmentVariable(key)) != null;
        }

        public static string GetOrThrow(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? throw new InvalidOperationException($"Environment variable {key} has not been set.");
        }
    }
}
