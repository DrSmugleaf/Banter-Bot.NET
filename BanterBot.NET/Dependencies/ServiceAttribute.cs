using System;

namespace BanterBot.NET.Dependencies
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute(ServiceScope scope = ServiceScope.Singleton)
        {
            Scope = scope;
        }

        public ServiceScope Scope { get; }
    }
}
