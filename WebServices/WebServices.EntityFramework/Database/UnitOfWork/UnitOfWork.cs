using DotLogix.WebServices.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Database; 

public class UnitOfWork : IUnitOfWork {
    public DbContext DbContext { get; }
    public IEntityTypeManager TypeManager { get; }
    public IEntityDatabaseOperations Operations { get; }
    public IEntityStateManager StateManager { get; }
    public IEntityEventManager EventManager { get; }

    public UnitOfWork(
        DbContext dbContext,
        IEntityTypeManager typeManager,
        IEntityDatabaseOperations operations,
        IEntityStateManager stateManager,
        IEntityEventManager eventManager
    ) {
        DbContext = dbContext;
        Operations = operations;
        StateManager = stateManager;
        EventManager = eventManager;
        TypeManager = typeManager;
    }

    public void Dispose() {
        DbContext.Dispose();
    }
}