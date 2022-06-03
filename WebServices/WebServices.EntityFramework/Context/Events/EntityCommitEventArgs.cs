using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.WebServices.EntityFramework.Context.Events {
    /// <summary>
    /// A class to represent entity commit event args
    /// </summary>
    public class EntityCommitEventArgs : EntityEventArgs {
        /// <summary>
        /// Creates a new instance of <see cref="EntityCommitEventArgs"/>
        /// </summary>
        public EntityCommitEventArgs(EntityEntry entityEntry)
            : base(entityEntry) {
        }
    }
}