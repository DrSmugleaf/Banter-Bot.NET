using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BanterBot.NET.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool TryFirst<T>(this IEnumerable<T> enumerable, [NotNullWhen(true)] out T? first)
        {
            return (first = enumerable.FirstOrDefault()) != null;
        }
    }
}
