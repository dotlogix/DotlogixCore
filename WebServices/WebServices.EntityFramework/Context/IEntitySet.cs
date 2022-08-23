// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntitySet.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.WebServices.EntityFramework.Context; 

/// <summary>
/// An interface for entity sets
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntitySet<TEntity> where TEntity : class {
    #region Add

    /// <summary>
    /// Add a single entity to the set
    /// </summary>
    TEntity Add(TEntity entity);

    /// <summary>
    /// Add a range of entities to the set
    /// </summary>
    ICollection<TEntity> AddRange(IEnumerable<TEntity> entities);

    #endregion

    #region Remove

    /// <summary>
    /// Remove a single entity from the set
    /// </summary>
    TEntity Remove(TEntity entity);

    /// <summary>
    /// Remove a range of entities from the set
    /// </summary>
    ICollection<TEntity> RemoveRange(IEnumerable<TEntity> entities);

    #endregion
        
    #region Query
    /// <summary>
    /// Create a linq style query to allow more advanced requests to the entity set
    /// </summary>
    IQueryable<TEntity> Query();
    /// <summary>
    /// Create a linq style query from raw sql
    /// </summary>
    IQueryable<TEntity> Query(string sql, object[] parameters);

    /// <summary>
    /// Create a linq style query from raw sql
    /// </summary>
    public IQueryable<TEntity> Query(FormattableString sql);
    #endregion
}