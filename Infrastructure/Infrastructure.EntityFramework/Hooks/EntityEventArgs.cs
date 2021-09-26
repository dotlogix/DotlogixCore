using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    public class EntityEventArgs : EventArgs {
        /// <summary>
        /// The entity entry
        /// </summary>
        public EntityEntry EntityEntry { get; }
        
        /// <summary>
        /// Creates a new instance of <see cref="EntityEventArgs"/>
        /// </summary>
        public EntityEventArgs(EntityEntry entityEntry) {
            EntityEntry = entityEntry;
        }
    }
}