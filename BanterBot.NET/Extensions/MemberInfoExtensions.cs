using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BanterBot.NET.Extensions
{
    public static class MemberInfoExtensions
    {
        public static bool HasAttribute<T>(this MemberInfo element) where T : Attribute
        {
            return element.GetCustomAttribute<T>() != null;
        }

        public static bool TryGetAttribute<T>(this MemberInfo element, [NotNullWhen(true)] out T? attribute)
            where T : Attribute
        {
            return (attribute = element.GetCustomAttribute<T>()) != null;
        }
    }
}
