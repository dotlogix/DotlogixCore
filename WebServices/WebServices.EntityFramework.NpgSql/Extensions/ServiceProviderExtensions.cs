#region
using System;
using System.Diagnostics.CodeAnalysis;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
#endregion

namespace DotLogix.WebServices.EntityFramework.Extensions; 

[SuppressMessage("ReSharper", "EF1001")]
public static class ServiceProviderExtensions {
    public static IServiceCollection AddNpgsqlEntityContext<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
        where TDbContext : DbContext {
        return services.AddNpgsqlEntityContext<TDbContext, EntityContext>(optionsAction);
    }
    public static IServiceCollection AddNpgsqlEntityContext<TDbContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        where TDbContext : DbContext {
        return services.AddNpgsqlEntityContext<TDbContext, EntityContext>(optionsAction);
    }
        
    public static IServiceCollection AddNpgsqlEntityContext<TDbContext, TEntityContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
        where TEntityContext : class, IEntityContext
        where TDbContext : DbContext {
        return services.AddNpgsqlEntityContext<TDbContext, TEntityContext>((_, optionsBuilder) => optionsAction?.Invoke(optionsBuilder));
    }
        
    public static IServiceCollection AddNpgsqlEntityContext<TDbContext, TEntityContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        where TEntityContext : class, IEntityContext
        where TDbContext : DbContext {
        services.TryAddScoped<IUnitOfWorkFactory, NpgsqlUnitOfWorkFactory<TDbContext>>();
        return services.AddEntityContext<TDbContext, TEntityContext>(optionsAction);
    }
}