using System;
using System.Collections.Generic;
using System.Linq;

namespace BanterBot.NET.Extensions
{
    public static class LinqExtensions
    {
        public static T? MaxBy<T, V>(this IEnumerable<T> source, Func<T, V> selector, IComparer<V>? comparer = null) where V : IComparable
        {
            comparer ??= Comparer<V>.Default;

            return source.Aggregate((x, y) =>
            {
                var comparison = comparer.Compare(selector(x), selector(y));

                return comparison switch
                {
                    var n when n < 0 => y,
                    var n when n > 0 => x,
                    _ => x
                };
            });
        }
    }
}
