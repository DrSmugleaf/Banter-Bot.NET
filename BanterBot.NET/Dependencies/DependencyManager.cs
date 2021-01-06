using System;
using System.Collections.Generic;
using System.Reflection;
using BanterBot.NET.Extensions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Victoria;

namespace BanterBot.NET.Dependencies
{
    public class DependencyManager : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services = new();

        private ServiceProvider ServiceProvider { get; }

        public DependencyManager(DiscordSocketClient client)
        {
            var collection = new ServiceCollection();

            foreach (var type in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (!type.HasAttribute<ServiceAttribute>())
                {
                    continue;
                }

                collection.AddSingleton(type, type);
            }

            collection
                .AddSingleton(client)
                .AddLavaNode(c =>
                {
                    c.SelfDeaf = false;
                    c.Port = 8080;
                });

            var options = new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            };

            ServiceProvider = collection.BuildServiceProvider(options);

            foreach (var descriptor in collection)
            {
                var type = descriptor.ServiceType;
                var instance = ServiceProvider.GetService(type);

                _services.Add(type, instance);

                if (!instance.GetType().HasAttribute<ServiceAttribute>())
                {
                    continue;
                }

                foreach (var field in instance.GetType().GetFields())
                {
                    if (!field.HasAttribute<ServiceDependencyAttribute>())
                    {
                        continue;
                    }

                    var serviceType = field.FieldType;
                    var service = _services[serviceType];
                    field.SetValue(instance, service);
                }

                foreach (var property in instance.GetType().GetProperties())
                {
                    if (!property.HasAttribute<ServiceDependencyAttribute>())
                    {
                        continue;
                    }

                    var serviceType = property.PropertyType;
                    var service = _services[serviceType];
                    property.SetValue(instance, service);
                }

                if (instance is IService iService)
                {
                    iService.AfterInject();
                }
            }
        }

        public object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }
    }
}
