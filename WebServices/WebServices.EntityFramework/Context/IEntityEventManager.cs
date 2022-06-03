using System;
using DotLogix.WebServices.EntityFramework.Context.Events;

namespace DotLogix.WebServices.EntityFramework.Context {
    public interface IEntityEventManager {
        EntityEvent<QueryCompileEventArgs> QueryCompile { get; }
        EntityEvent<QueryResultEventArgs> QueryExecuting { get; }
        EntityEvent<QueryResultEventArgs> QueryExecuted { get; }
        EntityEvent<QueryResultEventArgs> QueryFailed { get; }
        EntityEvent<EntityTrackEventArgs> Tracked { get; }
        EntityEvent<EntityStateEventArgs> StateChanged { get; }
        EntityEvent<EntityCommitEventArgs> Commit { get; }
        EntityEvent<EntityCommitEventArgs> Committed { get; }
        
        IEntityEventHandler GetHandler<TEntity>();
        IEntityEventHandler GetHandler(Type entityType);
        IEntityEventHandler GetHandler();
    }
}