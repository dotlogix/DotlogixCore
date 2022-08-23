using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.WebServices.EntityFramework.Context.Events; 

/// <summary>
/// A class to represent entity state event args
/// </summary>
public class EntityStateEventArgs : EntityEventArgs {
    /// <summary>
    /// The old entity state
    /// </summary>
    public EntityState OldState { get; }
    /// <summary>
    /// The new entity state
    /// </summary>
    public EntityState NewState { get; }

    /// <summary>
    /// Creates a new instance of <see cref="EntityCommitEventArgs"/>
    /// </summary>
    public EntityStateEventArgs(EntityEntry entityEntry, EntityState oldState, EntityState newState)
        : base(entityEntry) {
        OldState = oldState;
        NewState = newState;
    }
}