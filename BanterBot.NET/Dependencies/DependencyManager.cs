using System;
using System.Collections.Generic;
using System.Reflection;
using BanterBot.NET.Extensions;
using Microsoft.Extensions.DependencyInjection;
using YoutubeExplode;

namespace BanterBot.NET.Dependencies
{
    public class DependencyManager : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services = new();

        private ServiceProvider ServiceProvider { get; }

        public DependencyManager()
        {
            var collection = new ServiceCollection();

            foreach (var type in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (!type.HasAttribute<ServiceAttribute>())
                {
                    continue;
                }

                var instance = Activator.CreateInstance(type);

                if (instance == null)
                {
                    throw new InvalidOperationException();
                }

                collection.AddSingleton(type, instance);
            }

            collection.AddSingleton(new YoutubeClient());

            var options = new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            };

            foreach (var descriptor in collection)
            {
                _services.Add(descriptor.ServiceType, descriptor.ImplementationInstance);
            }

            ServiceProvider = collection.BuildServiceProvider(options);

            foreach (var descriptor in collection)
            {
                var instance = descriptor.ImplementationInstance;

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
