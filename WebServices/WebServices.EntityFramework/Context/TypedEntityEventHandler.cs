using System;
using DotLogix.WebServices.EntityFramework.Context.Events;

namespace DotLogix.WebServices.EntityFramework.Context {
    /// <summary>
    /// A class to represent entity framework event hooks
    /// </summary>
    public class TypedEntityEventHandler : IEntityEventHandler {
        private readonly EntityEventManager _manager;

        /// <inheritdoc />
        public event EventHandler<QueryCompileEventArgs> Query {
            add => _manager.QueryCompile.Subscribe(EntityType, value);
            remove => _manager.QueryCompile.Unsubscribe(EntityType, value);
        }
        
        
        /// <inheritdoc />
        public event EventHandler<QueryResultEventArgs> QueryResult {
            add => _manager.QueryExecuting.Subscribe(EntityType, value);
            remove => _manager.QueryExecuting.Unsubscribe(EntityType, value);
        }

        /// <inheritdoc />
        public event EventHandler<QueryResultEventArgs> QueryError {
            add => _manager.QueryFailed.Subscribe(EntityType, value);
            remove => _manager.QueryFailed.Unsubscribe(EntityType, value);
        }

        /// <inheritdoc />
        public event EventHandler<EntityTrackEventArgs> EntityTracked {
            add => _manager.Tracked.Subscribe(EntityType, value);
            remove => _manager.Tracked.Unsubscribe(EntityType, value);
        }

        /// <inheritdoc />
        public event EventHandler<EntityStateEventArgs> EntityStateChanged {
            add => _manager.StateChanged.Subscribe(EntityType, value);
            remove => _manager.StateChanged.Unsubscribe(EntityType, value);
        }
        
        /// <inheritdoc />
        public event EventHandler<EntityCommitEventArgs> EntityCommit {
            add => _manager.Commit.Subscribe(EntityType, value);
            remove => _manager.Commit.Unsubscribe(EntityType, value);
        }
        
        /// <inheritdoc />
        public event EventHandler<EntityCommitEventArgs> EntityCommitted {
            add => _manager.Committed.Subscribe(EntityType, value);
            remove => _manager.Committed.Unsubscribe(EntityType, value);
        }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TypedEntityEventHandler"/>
        /// </summary>
        public TypedEntityEventHandler(Type entityType, EntityEventManager manager) {
            _manager = manager;
            EntityType = entityType;
        }
    }
}