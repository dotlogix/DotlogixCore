// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Caching;
using DotLogix.WebServices.EntityFramework.Database;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.WebServices.EntityFramework.Context; 

/// <summary>
/// An interface to represent an entity context
/// </summary>
public interface IEntityContext : IEntitySetProvider, IDisposable {
    /// <summary>
    ///     The type manager of the entity context
    /// </summary>
    IEntityTypeManager TypeManager { get; }
        
    /// <summary>
    ///     The database operations of the entity context
    /// </summary>
    IEntityDatabaseOperations Operations { get; }
        
    /// <summary>
    ///     The state manager of the entity context
    /// </summary>
    IEntityStateManager StateManager { get; }

    /// <summary>
    ///     The event manager of the entity context
    /// </summary>
    IEntityEventManager EventManager { get; }

    /// <summary>
    ///     The global entity cache of the entity context
    /// </summary>
    ICache<object, object> EntityCache { get; }

    #region Attach
    /// <inheritdoc cref="IEntityStateManager.Attach(object)"/>
    void Attach(object entity);
        
    /// <inheritdoc cref="IEntityStateManager.Attach(object[])"/>
    void AttachRange(params object[] entities);
        
    /// <inheritdoc cref="IEntityStateManager.Attach(IEnumerable{object})"/>
    void AttachRange(IEnumerable<object> entities);
    #endregion
    #region Detach
    /// <inheritdoc cref="IEntityStateManager.Detach(object)"/>
    void Detach(object entity);
        
    /// <inheritdoc cref="IEntityStateManager.Detach(object[])"/>
    void DetachRange(params object[] entities);
        
    /// <inheritdoc cref="IEntityStateManager.Detach(IEnumerable{object})"/>
    void DetachRange(IEnumerable<object> entities);
    #endregion

    #region Add
    /// <inheritdoc cref="IEntityStateManager.MarkAdded(object)"/>
    void Add(object entity);
        
    /// <inheritdoc cref="IEntityStateManager.MarkAdded(object[])"/>
    void AddRange(params object[] entities);
        
    /// <inheritdoc cref="IEntityStateManager.MarkAdded(IEnumerable{object})"/>
    void AddRange(IEnumerable<object> entities);
    #endregion
    #region Update
    /// <inheritdoc cref="IEntityStateManager.MarkModified(object)"/>
    void Update(object entity);
        
    /// <inheritdoc cref="IEntityStateManager.MarkModified(object[])"/>
    void UpdateRange(params object[] entities);
        
    /// <inheritdoc cref="IEntityStateManager.MarkModified(IEnumerable{object})"/>
    void UpdateRange(IEnumerable<object> entities);
    #endregion
    #region Remove
    /// <inheritdoc cref="IEntityStateManager.MarkRemoved(object)"/>
    void Remove(object entity);
        
    /// <inheritdoc cref="IEntityStateManager.MarkRemoved(object[])"/>
    void RemoveRange(params object[] entities);
        
    /// <inheritdoc cref="IEntityStateManager.MarkRemoved(IEnumerable{object})"/>
    void RemoveRange(IEnumerable<object> entities);
    #endregion

    /// <inheritdoc cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)"/>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    /// <inheritdoc cref="DbContext.SaveChanges()"/>
    int Complete();
    void Reset();
}