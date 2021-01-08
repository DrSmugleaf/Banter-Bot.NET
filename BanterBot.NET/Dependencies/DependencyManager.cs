using System;
using System.Reflection;
using BanterBot.NET.Database;
using BanterBot.NET.Environments;
using BanterBot.NET.Extensions;
using BanterBot.NET.Logging;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Victoria;

namespace BanterBot.NET.Dependencies
{
    public class DependencyManager : IServiceProvider
    {
        private ServiceProvider ServiceProvider { get; }

        public DependencyManager(DiscordSocketClient client)
        {
            var collection = new ServiceCollection();

            DatabaseContext.IsMigration = false;
            MigrateAllContexts();

            foreach (var type in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (!type.TryGetAttribute(out ServiceAttribute? service))
                {
                    continue;
                }

                switch (service.Scope)
                {
                    case ServiceScope.Singleton:
                        collection.AddSingleton(type, type);
                        break;
                    case ServiceScope.Scoped:
                        collection.AddScoped(type, type);
                        break;
                    case ServiceScope.Transient:
                        collection.AddTransient(type, type);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            collection
                .AddSingleton(client)
                .AddLavaNode(c =>
                {
                    c.SelfDeaf = false;
                    c.Hostname = EnvironmentKey.Lavahost.GetOrThrow();
                    c.Port = 8080;
                });

            var options = new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            };

            ServiceProvider = collection.BuildServiceProvider(options);
            SetDependencies(collection);
        }

        private void MigrateAllContexts()
        {
            foreach (var type in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (!type.IsSubclassOf(typeof(DbContext)) ||
                    type.IsAbstract)
                {
                    continue;
                }

                Logger.DebugS($"Creating instance of {type} for migration.");

                var instance = (DbContext) (Activator.CreateInstance(type) ?? throw new InvalidOperationException());

                instance.Database.Migrate();
            }
        }

        private void SetDependencies(IServiceCollection collection)
        {
            foreach (var descriptor in collection)
            {
                var type = descriptor.ServiceType;

                if (!type.TryGetAttribute(out ServiceAttribute? serviceAttribute) ||
                    serviceAttribute.Scope == ServiceScope.Scoped)
                {
                    continue;
                }

                var instance = ServiceProvider.GetService(type);

                foreach (var field in instance.GetType().GetFields())
                {
                    if (!field.HasAttribute<ServiceDependencyAttribute>())
                    {
                        continue;
                    }

                    var serviceType = field.FieldType;
                    var service = ServiceProvider.GetService(serviceType);
                    field.SetValue(instance, service);
                }

                foreach (var property in instance.GetType().GetProperties())
                {
                    if (!property.HasAttribute<ServiceDependencyAttribute>())
                    {
                        continue;
                    }

                    var serviceType = property.PropertyType;
                    var service = ServiceProvider.GetService(serviceType);
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
