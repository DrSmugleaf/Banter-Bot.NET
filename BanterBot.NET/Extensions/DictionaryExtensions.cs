using System;
using System.Collections.Generic;

namespace BanterBot.NET.Extensions
{
    public static class DictionaryExtensions
    {
        public static V GetOrNew<K, V>(this IDictionary<K, V> dictionary, K key, Action<V>? onCreate = null)
            where K : notnull
            where V : new()
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = new V();
                onCreate?.Invoke(value);
                dictionary[key] = value;
            }

            return value;
        }
    }
}
