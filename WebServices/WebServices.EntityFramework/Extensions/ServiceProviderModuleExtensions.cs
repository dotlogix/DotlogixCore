using DotLogix.Infrastructure;
using DotLogix.Infrastructure.EntityFramework;
using DotLogix.WebServices.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.WebServices.EntityFramework.Extensions {
    public static class ServiceProviderModuleExtensions {
        public static IServiceCollection AddEntityContext<TService, TImplementation>(this IServiceCollection services)
            where TService : class, IEfEntityContext
            where TImplementation : class, TService
        {
            services.AddScoped<TService, TImplementation>();
            services.AddAlias<IEfEntityContext, TService>();
            services.AddAlias<IEntityContext, TService>();
            return services;
        }
    }
}
