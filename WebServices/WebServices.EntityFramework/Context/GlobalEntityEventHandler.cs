using System;
using DotLogix.WebServices.EntityFramework.Context.Events;

namespace DotLogix.WebServices.EntityFramework.Context {
    /// <summary>
    /// A class to represent entity framework event hooks
    /// </summary>
    public class GlobalEntityEventHandler : IEntityEventHandler {
        private readonly EntityEventManager _manager;

        /// <inheritdoc />
        public event EventHandler<QueryCompileEventArgs> Query {
            add => _manager.QueryCompile.Subscribe(value);
            remove => _manager.QueryCompile.Unsubscribe(value);
        }
        
        
        /// <inheritdoc />
        public event EventHandler<QueryResultEventArgs> QueryResult {
            add => _manager.QueryExecuting.Subscribe(value);
            remove => _manager.QueryExecuting.Unsubscribe(value);
        }
        
        /// <inheritdoc />
        public event EventHandler<QueryResultEventArgs> QueryError {
            add => _manager.QueryFailed.Subscribe(value);
            remove => _manager.QueryFailed.Unsubscribe(value);
        }

        /// <inheritdoc />
        public event EventHandler<EntityTrackEventArgs> EntityTracked {
            add => _manager.Tracked.Subscribe(value);
            remove => _manager.Tracked.Unsubscribe(value);
        }

        /// <inheritdoc />
        public event EventHandler<EntityStateEventArgs> EntityStateChanged {
            add => _manager.StateChanged.Subscribe(value);
            remove => _manager.StateChanged.Unsubscribe(value);
        }
        
        /// <inheritdoc />
        public event EventHandler<EntityCommitEventArgs> EntityCommit {
            add => _manager.Commit.Subscribe(value);
            remove => _manager.Commit.Unsubscribe(value);
        }
        
        /// <inheritdoc />
        public event EventHandler<EntityCommitEventArgs> EntityCommitted {
            add => _manager.Committed.Subscribe(value);
            remove => _manager.Committed.Unsubscribe(value);
        }

        /// <inheritdoc />
        public Type EntityType { get; } = typeof(void);

        /// <summary>
        /// Creates a new instance of <see cref="GlobalEntityEventHandler"/>
        /// </summary>
        public GlobalEntityEventHandler(EntityEventManager manager) {
            _manager = manager;
        }
    }
}