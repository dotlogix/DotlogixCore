// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
	/// <summary>
	/// A non generic marker interface for repository types
	/// </summary>
	public interface IRepository
	{

	}

    /// <summary>
    /// A non generic marker interface for repository types
    /// </summary>
    public interface IRepository<TEntity> : IRepository where TEntity : class, new() {
        /// <summary>
        /// Get all entities
        /// </summary>
        ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all entities matching an expression
        /// </summary>
        ValueTask<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a single entity to the set
        /// </summary>
        ValueTask<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Add a range of entities to the set
        /// </summary>
        ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Remove a single entity from the set
        /// </summary>
        ValueTask<TEntity> RemoveAsync(TEntity entity);

        /// <summary>
        /// Remove a range of entities from the set
        /// </summary>
        ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities);
    }

    /// <summary>
    /// A generic basic interface for repository types with strongly typed key
	/// </summary>
	public interface IRepository<in TKey, TEntity> : IRepository<TEntity> where TEntity : class, new() {
		/// <summary>
		/// Get a single entity by key
		/// </summary>
		ValueTask<TEntity> GetAsync(TKey key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Get a range of entities by key
		/// </summary>
		ValueTask<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
    }
}
