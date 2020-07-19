using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// A class to represent entity framework event hooks
    /// </summary>
    public class EfEntityHooks {
        private readonly EfEntityHookListeners _efEntityHookListeners;

        /// <summary>
        /// Occurs if an entity is newly tracked by the db context
        /// </summary>
        public event EventHandler<EntityTrackedEventArgs> EntityTracked {
            add => _efEntityHookListeners.EntityTracked.Add(EntityType, value);
            remove => _efEntityHookListeners.EntityTracked.Remove(EntityType, value);
        }

        /// <summary>
        /// Occurs each time an entity changes it's state. If an entity is newly added to the db context it is only track-able with the <see cref="EntityTracked"/> event
        /// </summary>
        public event EventHandler<EntityStateChangedEventArgs> EntityStateChanged {
            add => _efEntityHookListeners.EntityStateChanged.Add(EntityType, value);
            remove => _efEntityHookListeners.EntityStateChanged.Remove(EntityType, value);
        }
        
        /// <summary>
        /// Occurs each time before the user commits the changes of an entity to the database
        /// </summary>
        public event EventHandler<EntityCommitEventArgs> EntityCommit {
            add => _efEntityHookListeners.EntityCommit.Add(EntityType, value);
            remove => _efEntityHookListeners.EntityCommit.Remove(EntityType, value);
        }
        
        /// <summary>
        /// Occurs each time the user committed the changes of an entity to the database
        /// </summary>
        public event EventHandler<EntityCommitEventArgs> EntityCommitted {
            add => _efEntityHookListeners.EntityCommitted.Add(EntityType, value);
            remove => _efEntityHookListeners.EntityCommitted.Remove(EntityType, value);
        }

        /// <summary>
        /// The entity clr type
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EfEntityHooks"/>
        /// </summary>
        public EfEntityHooks(Type entityType, EfEntityHookListeners efEntityHookListeners) {
            _efEntityHookListeners = efEntityHookListeners;
            EntityType = entityType;
        }
    }
}