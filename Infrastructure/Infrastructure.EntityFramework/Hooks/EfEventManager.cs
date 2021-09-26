using System;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    public class EfEventManager : IEfEventManager {
        public EfEventManager() {
            QueryCompile = new EventHandlerCollection<QueryCompileEventArgs>();
            QueryExecuting = new EventHandlerCollection<QueryResultEventArgs>();
            QueryExecuted = new EventHandlerCollection<QueryResultEventArgs>();
            QueryFailed = new EventHandlerCollection<QueryResultEventArgs>();
            Tracked = new EventHandlerCollection<EntityTrackEventArgs>();
            StateChanged = new EventHandlerCollection<EntityStateEventArgs>();
            Commit = new EventHandlerCollection<EntityCommitEventArgs>();
            Committed = new EventHandlerCollection<EntityCommitEventArgs>();
        }

        public EventHandlerCollection<QueryCompileEventArgs> QueryCompile { get; }
        public EventHandlerCollection<QueryResultEventArgs> QueryExecuting { get; }
        public EventHandlerCollection<QueryResultEventArgs> QueryExecuted { get; }
        public EventHandlerCollection<QueryResultEventArgs> QueryFailed { get; }
        public EventHandlerCollection<EntityTrackEventArgs> Tracked { get; }
        public EventHandlerCollection<EntityStateEventArgs> StateChanged { get; }
        public EventHandlerCollection<EntityCommitEventArgs> Commit { get; }
        public EventHandlerCollection<EntityCommitEventArgs> Committed { get; }

        public IEfEventHandler GetHandler<TEntity>() {
            return GetHandler(typeof(TEntity));
        }

        public IEfEventHandler GetHandler(Type entityType) {
            return entityType != typeof(void)
                       ? new TypedEfEventHandler(entityType, this)
                       : GetHandler();
        }
        public IEfEventHandler GetHandler() {
            return new GlobalEfEventHandler(this);
        }
    }
}