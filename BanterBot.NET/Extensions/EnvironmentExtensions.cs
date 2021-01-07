using System;
using System.Diagnostics.CodeAnalysis;

namespace BanterBot.NET.Extensions
{
    public static class EnvironmentExtensions
    {
        public static bool TryGetEnvironmentVariable(string key, [NotNullWhen(true)] out string? value)
        {
            return (value = Environment.GetEnvironmentVariable(key)) != null;
        }
    }
}
