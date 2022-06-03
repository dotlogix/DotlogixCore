using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.WebServices.EntityFramework.Context.Events {
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