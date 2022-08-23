using System;
using System.Diagnostics.CodeAnalysis;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

[SuppressMessage("ReSharper", "EF1001")]
public static class ServiceProviderExtensions {
    public static IServiceCollection AddEntityContext<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
        where TDbContext : DbContext {
        return services.AddEntityContext<TDbContext, EntityContext>(optionsAction);
    }
        
    public static IServiceCollection AddEntityContext<TDbContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        where TDbContext : DbContext {
        return services.AddEntityContext<TDbContext, EntityContext>(optionsAction);
    }
        
    public static IServiceCollection AddEntityContext<TDbContext, TEntityContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
        where TEntityContext : class, IEntityContext
        where TDbContext : DbContext {
        return AddEntityContext<TDbContext, TEntityContext>(services, (_, optionsBuilder) => optionsAction?.Invoke(optionsBuilder));
    }
        
    public static IServiceCollection AddEntityContext<TDbContext, TEntityContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        where TEntityContext : class, IEntityContext
        where TDbContext : DbContext
    {
        services.AddDbContextFactory<TDbContext>(optionsAction);
        services.TryAddScoped<IUnitOfWorkFactory, UnitOfWorkFactory<TDbContext>>();
        services.TryAddScoped<IEntityContext, TEntityContext>();
        return services;
    }
}