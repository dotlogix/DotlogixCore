using System;
using DotLogix.WebServices.EntityFramework.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

public static class DbContextOptionsBuilderExtensions {
    public static DbContextOptionsBuilder UseQueryExtensions(this DbContextOptionsBuilder builder, Action<QueryEnhancementOptions> configure = null) {
        return UseExtension(builder, configure);
    }
    public static DbContextOptionsBuilder UseNamingConventions(this DbContextOptionsBuilder builder, Action<NamingConventionsExtensionOptions> configure = null) {
        return UseExtension(builder, configure);
    }
    
    public static DbContextOptionsBuilder UseExtension<T>(this DbContextOptionsBuilder optionsBuilder, Action<T> configure = null)
        where T : class, IDbContextExtensionOptions, new()
    {
        var extension = optionsBuilder.Options.FindExtension<DbContextOptionsExtension<T>>() ?? new DbContextOptionsExtension<T>();
        if (configure is not null)
            extension = extension.WithOptions(configure);
        ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);
        return optionsBuilder;
    }
}