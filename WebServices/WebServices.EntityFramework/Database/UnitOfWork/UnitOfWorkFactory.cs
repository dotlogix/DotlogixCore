using System.Diagnostics.CodeAnalysis;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Context.Events;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Database; 

[SuppressMessage("ReSharper", "EF1001")]
public class UnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory where TDbContext : DbContext {
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<TDbContext> dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public IUnitOfWork Create() {
        var dbContext = CreateDbContext();
        return new UnitOfWork(
            dbContext,
            CreateTypeManager(dbContext),
            CreateDatabaseOperations(dbContext),
            CreateStateManager(dbContext),
            CreateEventManager(dbContext)
        );
    }

    protected virtual TDbContext CreateDbContext() {
        return _dbContextFactory.CreateDbContext();
    }

    protected virtual IEntityTypeManager CreateTypeManager(TDbContext dbContext) {
        return new EntityTypeManager(dbContext.Model);
    }

    protected virtual IEntityStateManager CreateStateManager(TDbContext dbContext) {
        return new EntityStateManager(dbContext.GetStateManager());
    }

    protected virtual IEntityDatabaseOperations CreateDatabaseOperations(TDbContext dbContext) {
        return new RelationalEntityDbOperations(dbContext);
    }

    protected virtual IEntityEventManager CreateEventManager(TDbContext dbContext) {
        return new EntityEventManager();
    }
}