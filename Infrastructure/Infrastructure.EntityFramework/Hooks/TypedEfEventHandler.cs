using System;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    /// <summary>
    /// A class to represent entity framework event hooks
    /// </summary>
    public class TypedEfEventHandler : IEfEventHandler {
        private readonly EfEventManager _manager;

        /// <inheritdoc />
        public event EventHandler<QueryCompileEventArgs> Query {
            add => _manager.QueryCompile.AddHandler(EntityType, value);
            remove => _manager.QueryCompile.RemoveHandler(EntityType, value);
        }
        
        
        /// <inheritdoc />
        public event EventHandler<QueryResultEventArgs> QueryResult {
            add => _manager.QueryExecuting.AddHandler(value);
            remove => _manager.QueryExecuting.RemoveHandler(value);
        }

        /// <inheritdoc />
        public event EventHandler<QueryResultEventArgs> QueryError {
            add => _manager.QueryFailed.AddHandler(value);
            remove => _manager.QueryFailed.RemoveHandler(value);
        }

        /// <inheritdoc />
        public event EventHandler<EntityTrackEventArgs> EntityTracked {
            add => _manager.Tracked.AddHandler(EntityType, value);
            remove => _manager.Tracked.RemoveHandler(EntityType, value);
        }

        /// <inheritdoc />
        public event EventHandler<EntityStateEventArgs> EntityStateChanged {
            add => _manager.StateChanged.AddHandler(EntityType, value);
            remove => _manager.StateChanged.RemoveHandler(EntityType, value);
        }
        
        /// <inheritdoc />
        public event EventHandler<EntityCommitEventArgs> EntityCommit {
            add => _manager.Commit.AddHandler(EntityType, value);
            remove => _manager.Commit.RemoveHandler(EntityType, value);
        }
        
        /// <inheritdoc />
        public event EventHandler<EntityCommitEventArgs> EntityCommitted {
            add => _manager.Committed.AddHandler(EntityType, value);
            remove => _manager.Committed.RemoveHandler(EntityType, value);
        }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TypedEfEventHandler"/>
        /// </summary>
        public TypedEfEventHandler(Type entityType, EfEventManager manager) {
            _manager = manager;
            EntityType = entityType;
        }
    }
}