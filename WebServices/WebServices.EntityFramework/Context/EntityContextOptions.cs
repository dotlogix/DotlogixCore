using System;
using DotLogix.Core.Diagnostics;

namespace DotLogix.WebServices.EntityFramework.Context; 

public class EntityContextOptions {
    public IServiceProvider ServiceProvider { get; private set; }
    public ILogSourceProvider LogSourceProvider { get; private set; }
    public Type DbContextType { get; private set; }

    public EntityContextOptions(IServiceProvider serviceProvider) {
        ServiceProvider = serviceProvider;
    }

    public void UseServiceProvider(IServiceProvider serviceProvider) {
        ServiceProvider = serviceProvider;
    }
        
    public void UseDbContext(Type dbContextType) {
        DbContextType = dbContextType;
    }
        
    public void UseDbContext<TDbContext>() {
        UseDbContext(typeof(TDbContext));
    }
        
    public void UseLogSourceProvider(ILogSourceProvider logSourceProvider) {
        LogSourceProvider = logSourceProvider;
    }
}