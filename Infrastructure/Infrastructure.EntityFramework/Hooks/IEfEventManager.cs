using System;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    public interface IEfEventManager {
        EventHandlerCollection<QueryCompileEventArgs> QueryCompile { get; }
        EventHandlerCollection<QueryResultEventArgs> QueryExecuting { get; }
        EventHandlerCollection<QueryResultEventArgs> QueryExecuted { get; }
        EventHandlerCollection<QueryResultEventArgs> QueryFailed { get; }
        EventHandlerCollection<EntityTrackEventArgs> Tracked { get; }
        EventHandlerCollection<EntityStateEventArgs> StateChanged { get; }
        EventHandlerCollection<EntityCommitEventArgs> Commit { get; }
        EventHandlerCollection<EntityCommitEventArgs> Committed { get; }
        
        IEfEventHandler GetHandler<TEntity>();
        IEfEventHandler GetHandler(Type entityType);
        IEfEventHandler GetHandler();
    }
}