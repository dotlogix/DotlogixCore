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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    /// <summary>
    /// An interface for entity sets
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntitySet<TEntity> {
        #region Query
        /// <summary>
        /// Create a linq style query to allow more advanced requests to the entity set
        /// </summary>
        IQuery<TEntity> Query();
        #endregion

        #region Get
        /// <summary>
        /// Get a single entity by key
        /// </summary>
        ValueTask<TEntity> GetAsync(object key, CancellationToken cancellationToken = default);
		/// <summary>
		/// Get a range of entities by id
		/// </summary>
		ValueTask<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<object> keys, CancellationToken cancellationToken = default);
		/// <summary>
		/// Get all entities
		/// </summary>
		ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Get all entities matching an expression
		/// </summary>
		ValueTask<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default);
        #endregion

        #region Add
        /// <summary>
        /// Add a single entity to the set
        /// </summary>
        ValueTask<TEntity> AddAsync(TEntity entity);
		/// <summary>
		/// Add a range of entities to the set
		/// </summary>
		ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
		#endregion

		#region Remove
		/// <summary>
		/// Remove a single entity from the set
		/// </summary>
		ValueTask<TEntity> RemoveAsync(TEntity entity);
		/// <summary>
		/// Remove a range of entities from the set
		/// </summary>
		ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities);
		#endregion

		#region ReAttach
		/// <summary>
		/// Reattach a single entity to the underlying change tracker
		/// </summary>
		ValueTask<TEntity> ReAttachAsync(TEntity entity);
		/// <summary>
		/// Reattach a range of entities to the underlying change tracker
		/// </summary>
		ValueTask<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities);
        #endregion
    }
}
