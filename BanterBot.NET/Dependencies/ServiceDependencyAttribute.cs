using System;

namespace BanterBot.NET.Dependencies
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ServiceDependencyAttribute : Attribute
    {
    }
}
