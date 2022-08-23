using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.WebServices.EntityFramework.Context.Events; 

/// <summary>
/// A class to represent entity track event args
/// </summary>
public class EntityTrackEventArgs : EntityEventArgs {
    /// <summary>
    ///     <c>True</c> if the entity is being tracked as part of a database query; <c>false</c> otherwise.
    /// </summary>
    public bool FromQuery { get; }

    /// <summary>
    /// Creates a new instance of <see cref="EntityCommitEventArgs"/>
    /// </summary>
    public EntityTrackEventArgs(EntityEntry entityEntry, bool fromQuery)
        : base(entityEntry) {
        FromQuery = fromQuery;
    }
}