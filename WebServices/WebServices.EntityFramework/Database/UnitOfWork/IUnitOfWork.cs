using System;
using DotLogix.WebServices.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Database; 

/// <summary>
/// An interface to represent <see cref="IEntityContext"/> modules
/// </summary>
public interface IUnitOfWork : IDisposable {
    /// <summary>
    /// The current <see cref="DbContext"/> of this unit of work
    /// </summary>
    DbContext DbContext { get; }

    /// <summary>
    /// The <see cref="IEntityTypeManager"/> of this unit of work
    /// </summary>
    IEntityTypeManager TypeManager { get; }
        
    /// <summary>
    /// The <see cref="IEntityDatabaseOperations"/> of this unit of work
    /// </summary>
    IEntityDatabaseOperations Operations { get; }
        
    /// <summary>
    /// The <see cref="IEntityStateManager"/> of this unit of work
    /// </summary>
    IEntityStateManager StateManager { get; }
        
    /// <summary>
    /// The <see cref="IEntityEventManager"/> of this unit of work
    /// </summary>
    IEntityEventManager EventManager { get; }
}