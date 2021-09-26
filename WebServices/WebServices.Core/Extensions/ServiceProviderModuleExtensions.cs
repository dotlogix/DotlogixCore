using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Core.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ILogger = DotLogix.Core.Diagnostics.ILogger;

namespace DotLogix.WebServices.Core.Extensions {
    public static class ServiceProviderModuleExtensions {
        public static IServiceCollection AddLogSourceProvider(this IServiceCollection services) {
            return AddLogSourceProvider<ConfigLogSourceProvider>(services);
        }
        
        public static IServiceCollection AddLogSourceProvider<TProvider>(this IServiceCollection services) where TProvider : class, ILogSourceProvider {
            services.AddSingleton<ILogger>(BroadcastLogger.Instance);
            services.AddSingleton<ILogSourceProvider, TProvider>();
            services.Add(ServiceDescriptor.Describe(typeof(ILogSource<>), typeof(LogSource<>), ServiceLifetime.Singleton));

            services.AddLogging();
            services.RemoveAll<ILoggerProvider>();
            services.AddSingleton<ILoggerProvider, LoggerAdapterProvider>();
            return services;
        }
    }
}