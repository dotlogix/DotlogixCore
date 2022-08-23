#region
using System;
using DotLogix.WebServices.EntityFramework.Context.Events;
#endregion

namespace DotLogix.WebServices.EntityFramework.Context; 

public class EntityEventManager : IEntityEventManager {
    /// <inheritdoc />
    public EntityEvent<QueryCompileEventArgs> QueryCompile { get; }

    /// <inheritdoc />
    public EntityEvent<QueryResultEventArgs> QueryExecuting { get; }

    /// <inheritdoc />
    public EntityEvent<QueryResultEventArgs> QueryExecuted { get; }

    /// <inheritdoc />
    public EntityEvent<QueryResultEventArgs> QueryFailed { get; }

    /// <inheritdoc />
    public EntityEvent<EntityTrackEventArgs> Tracked { get; }

    /// <inheritdoc />
    public EntityEvent<EntityStateEventArgs> StateChanged { get; }

    /// <inheritdoc />
    public EntityEvent<EntityCommitEventArgs> Commit { get; }

    /// <inheritdoc />
    public EntityEvent<EntityCommitEventArgs> Committed { get; }

    public EntityEventManager() {
        QueryCompile = new EntityEvent<QueryCompileEventArgs>();
        QueryExecuting = new EntityEvent<QueryResultEventArgs>();
        QueryExecuted = new EntityEvent<QueryResultEventArgs>();
        QueryFailed = new EntityEvent<QueryResultEventArgs>();
        Tracked = new EntityEvent<EntityTrackEventArgs>();
        StateChanged = new EntityEvent<EntityStateEventArgs>();
        Commit = new EntityEvent<EntityCommitEventArgs>();
        Committed = new EntityEvent<EntityCommitEventArgs>();
    }

    /// <inheritdoc />
    public IEntityEventHandler GetHandler<TEntity>() {
        return GetHandler(typeof(TEntity));
    }

    /// <inheritdoc />
    public IEntityEventHandler GetHandler(Type entityType) {
        return entityType != typeof(void)
            ? new TypedEntityEventHandler(entityType, this)
            : GetHandler();
    }

    /// <inheritdoc />
    public IEntityEventHandler GetHandler() {
        return new GlobalEntityEventHandler(this);
    }
}