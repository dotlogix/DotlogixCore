using System;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    /// <summary>
    /// A class to represent entity framework event hooks
    /// </summary>
    public class GlobalEfEventHandler : IEfEventHandler {
        private readonly EfEventManager _manager;

        /// <inheritdoc />
        public event EventHandler<QueryCompileEventArgs> Query {
            add => _manager.QueryCompile.AddHandler(value);
            remove => _manager.QueryCompile.RemoveHandler(value);
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
            add => _manager.Tracked.AddHandler(value);
            remove => _manager.Tracked.RemoveHandler(value);
        }

        /// <inheritdoc />
        public event EventHandler<EntityStateEventArgs> EntityStateChanged {
            add => _manager.StateChanged.AddHandler(value);
            remove => _manager.StateChanged.RemoveHandler(value);
        }
        
        /// <inheritdoc />
        public event EventHandler<EntityCommitEventArgs> EntityCommit {
            add => _manager.Commit.AddHandler(value);
            remove => _manager.Commit.RemoveHandler(value);
        }
        
        /// <inheritdoc />
        public event EventHandler<EntityCommitEventArgs> EntityCommitted {
            add => _manager.Committed.AddHandler(value);
            remove => _manager.Committed.RemoveHandler(value);
        }

        /// <inheritdoc />
        public Type EntityType { get; } = typeof(void);

        /// <summary>
        /// Creates a new instance of <see cref="GlobalEfEventHandler"/>
        /// </summary>
        public GlobalEfEventHandler(EfEventManager manager) {
            _manager = manager;
        }
    }
}