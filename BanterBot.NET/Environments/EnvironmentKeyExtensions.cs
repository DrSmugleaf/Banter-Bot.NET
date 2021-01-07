using System;
using System.Diagnostics.CodeAnalysis;

namespace BanterBot.NET.Environments
{
    public static class EnvironmentKeyExtensions
    {
        public static string? Get(this EnvironmentKey key)
        {
            return Environment.GetEnvironmentVariable(key.ToString());
        }

        public static bool TryGet(this EnvironmentKey key, [NotNullWhen(true)] out string? value)
        {
            return EnvironmentExtensions.TryGetEnvironmentVariable(key.ToString(), out value);
        }

        public static string GetOrThrow(this EnvironmentKey key)
        {
            return EnvironmentExtensions.GetOrThrow(key.ToString());
        }
    }
}
