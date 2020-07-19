using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// A class to represent entity commit event args
    /// </summary>
    public class EntityCommitEventArgs : EventArgs{
        /// <summary>
        /// The entity entry
        /// </summary>
        public EntityEntry EntityEntry { get; }
        /// <summary>
        /// Creates a new instance of <see cref="EntityCommitEventArgs"/>
        /// </summary>
        public EntityCommitEventArgs(EntityEntry entityEntry) {
            EntityEntry = entityEntry;
        }
    }
}