using System;
using DotLogix.WebServices.EntityFramework.Context.Events;

namespace DotLogix.WebServices.EntityFramework.Context; 

public interface IEntityEventHandler {
    /// <summary>
    /// Invoked before a db query is executed by e. Allows modification of a query expression and query result
    /// </summary>
    event EventHandler<QueryCompileEventArgs> Query;
    /// <summary>
    /// Invoked after a db query is executed by ef. Allows modification of a query result
    /// </summary>
    event EventHandler<QueryResultEventArgs> QueryResult;
        
    /// <summary>
    /// Invoked after a db query by ef failed. Allows modification of a query result
    /// </summary>
    event EventHandler<QueryResultEventArgs> QueryError;
        
    /// <summary>
    /// Invoked when an entity is tracked by ef
    /// </summary>
    event EventHandler<EntityTrackEventArgs> EntityTracked;

    /// <summary>
    /// Invoked each time an entity changes it's state. If an entity is newly added to the db context it is only track-able with the <see cref="IEntityEventHandler.EntityTracked"/> event
    /// </summary>
    event EventHandler<EntityStateEventArgs> EntityStateChanged;

    /// <summary>
    /// Occurs each time before the user commits the changes of an entity to the database
    /// </summary>
    event EventHandler<EntityCommitEventArgs> EntityCommit;

    /// <summary>
    /// Occurs each time the user committed the changes of an entity to the database
    /// </summary>
    event EventHandler<EntityCommitEventArgs> EntityCommitted;

    /// <summary>
    /// The entity clr type
    /// </summary>
    Type EntityType { get; }
}