#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Core.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ILogger = DotLogix.Core.Diagnostics.ILogger;
#endregion

namespace DotLogix.WebServices.Core.Extensions; 

public static class ServiceProviderModuleExtensions {
    public static IServiceCollection AddLogTarget<TTarget>(this IServiceCollection services) where TTarget : class, ILogTarget {
        TryAddCoreServices(services);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ILogTarget, TTarget>());
        return services;
    }
    public static IServiceCollection AddLogTarget<TTarget>(this IServiceCollection services, Func<IServiceProvider, TTarget> implementationFactory) where TTarget : class, ILogTarget {
        TryAddCoreServices(services);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ILogTarget, TTarget>(implementationFactory));
        return services;
    }
        
    public static IServiceCollection AddAsyncLogTarget<TTarget>(this IServiceCollection services) where TTarget : class, IAsyncLogTarget {
        TryAddCoreServices(services);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IAsyncLogTarget, TTarget>());
        return services;
    }
        
    public static IServiceCollection AddAsyncLogTarget<TTarget>(this IServiceCollection services, Func<IServiceProvider, TTarget> implementationFactory) where TTarget : class, IAsyncLogTarget {
        TryAddCoreServices(services);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IAsyncLogTarget, TTarget>(implementationFactory));
        return services;
    }
        
    public static IServiceCollection AddLogSourceProvider(this IServiceCollection services) {
        return AddLogSourceProvider<ConfigLogSourceProvider>(services);
    }

    public static IServiceCollection AddLogSourceProvider<TProvider>(this IServiceCollection services)
        where TProvider : class, ILogSourceProvider {
        services.RemoveAll<ILogSourceProvider>();
        services.AddSingleton<ILogSourceProvider, TProvider>();
        TryAddCoreServices(services);
        return services;
    }

    private static void TryAddCoreServices(IServiceCollection services) {
        services.TryAddSingleton<ILogger>(provider => {
                var loggers = provider.GetRequiredService<IEnumerable<ILogTarget>>();
                var asyncLoggers = provider.GetRequiredService<IEnumerable<IAsyncLogTarget>>();
                return new BroadcastLogger(loggers, asyncLoggers);
            }
        );
        services.TryAddSingleton(typeof(ILogSource<>), typeof(LogSource<>));

        services.AddLogging();
        services.RemoveAll<ILoggerProvider>();
        services.AddSingleton<ILoggerProvider, LoggerAdapterProvider>();
    }
}