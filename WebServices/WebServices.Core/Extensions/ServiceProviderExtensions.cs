using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotLogix.WebServices.Core.Extensions {
    public static class ServiceProviderExtensions {
        public static T GetService<T>(this IServiceProvider provider, bool required) {
            return required ? provider.GetRequiredService<T>() : provider.GetService<T>();
        }
        public static void AddAlias<TAlias, TService>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient) where TService : TAlias {
            var serviceDescriptor = ServiceDescriptor.Describe(typeof(TAlias), p => p.GetService(typeof(TService)), lifetime); services.Add(serviceDescriptor);
            services.Add(serviceDescriptor);
        }
        
        public static void TryAddAlias<TAlias, TService>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient) where TService : TAlias {
            var serviceDescriptor = ServiceDescriptor.Describe(typeof(TAlias), p => p.GetService(typeof(TService)), lifetime); services.Add(serviceDescriptor);
            services.TryAdd(serviceDescriptor);
        }

        public static void AddAlias(this IServiceCollection services, Type aliasType, Type serviceType, ServiceLifetime lifetime = ServiceLifetime.Transient) {
            if(aliasType == null)
                throw new ArgumentNullException(nameof(aliasType));

            if(serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if(aliasType.IsAssignableFrom(serviceType) == false)
                throw new ArgumentException($"Target type {aliasType.Name} is not assignable from source type {serviceType.Name}");

            var descriptor = ServiceDescriptor.Describe(aliasType, p => p.GetService(serviceType), lifetime);
            services.Add(descriptor);
        }
        
        public static void TryAddAlias(this IServiceCollection services, Type aliasType, Type serviceType, ServiceLifetime lifetime = ServiceLifetime.Transient) {
            if(aliasType == null)
                throw new ArgumentNullException(nameof(aliasType));

            if(serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if(aliasType.IsAssignableFrom(serviceType) == false)
                throw new ArgumentException($"Target type {aliasType.Name} is not assignable from source type {serviceType.Name}");

            var descriptor = ServiceDescriptor.Describe(aliasType, p => p.GetService(serviceType), lifetime);
            services.TryAdd(descriptor);
        }
        
        public static void AddWithInterfaces<TService, TImplementation, TLimit>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TImplementation : TService
            where TService : class, TLimit {
            services.AddWithInterfaces(typeof(TService), typeof(TImplementation), typeof(TLimit), lifetime);
        }

        private static void AddWithInterfaces(this IServiceCollection services, Type serviceType, Type implementationType, Type limitType, ServiceLifetime lifetime = ServiceLifetime.Transient) {
            var serviceTypes = serviceType
                              .GetInterfaces()
                              .Where(type => 
                                         type != limitType
                                      && type != implementationType
                                      && limitType.IsAssignableFrom(type)
                                    )
                              .ToList();

            var serviceDescriptor = ServiceDescriptor.Describe(serviceType, implementationType, lifetime);
            services.Add(serviceDescriptor);
            foreach(var baseType in serviceTypes) {
                services.AddAlias(baseType, implementationType);
            }
        }
        
        public static void Add(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime = ServiceLifetime.Transient) {
            var serviceDescriptor = ServiceDescriptor.Describe(serviceType, implementationType, lifetime);
            services.Add(serviceDescriptor);
        }
    }
}
