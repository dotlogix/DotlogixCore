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
using DotLogix.WebServices.Core;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Database;
#endregion

namespace DotLogix.WebServices.EntityFramework.Repositories; 

/// <summary>
/// A non generic marker interface for repository types
/// </summary>
public interface IRepository
{

}

/// <summary>
/// A non generic marker interface for repository types
/// </summary>
public interface IRepository<TEntity> : IRepository where TEntity : class {
	/// <summary>
	/// Get all entities
	/// </summary>
	Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all entities matching an expression
	/// </summary>
	Task<int> CountAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all entities matching a query filter
	/// </summary>
	Task<int> CountAsync(IEntityQuery<TEntity, TEntity> filter, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all entities matching an expression
	/// </summary>
	Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all entities matching a query filter
	/// </summary>
	Task<bool> AnyAsync(IEntityQuery<TEntity, TEntity> filter, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all entities matching an expression
	/// </summary>
	Task<ICollection<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get all entities matching a query filter
	/// </summary>
	Task<ICollection<TEntity>> WhereAsync(IEntityQuery<TEntity, TEntity> filter, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get paged entities matching a query filter
	/// </summary>
	Task<PaginationResult<TEntity>> GetPagedAsync(PaginationTerm pagination, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get paged entities matching a query filter
	/// </summary>
	Task<PaginationResult<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filterExpression, PaginationTerm pagination, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get paged entities matching a query filter
	/// </summary>
	Task<PaginationResult<TEntity>> GetPagedAsync(IEntityQuery<TEntity, TEntity> queryModifier, PaginationTerm pagination, CancellationToken cancellationToken = default);
        
	/// <summary>
	/// Add a single entity to the set
	/// </summary>
	TEntity Add(TEntity entity);

	/// <summary>
	/// Add a range of entities to the set
	/// </summary>
	ICollection<TEntity> AddRange(IEnumerable<TEntity> entities);

	/// <summary>
	/// Remove a single entity from the set
	/// </summary>
	TEntity Remove(TEntity entity);

	/// <summary>
	/// Remove a range of entities from the set
	/// </summary>
	ICollection<TEntity> RemoveRange(IEnumerable<TEntity> entities);
}

/// <summary>
/// A generic basic interface for repository types with strongly typed key
/// </summary>
public interface IRepository<in TKey, TEntity> : IRepository<TEntity> where TEntity : class {
	/// <summary>
	/// Get a single entity by key
	/// </summary>
	Task<TEntity> GetAsync(TKey key, CacheOptions cacheOptions = CacheOptions.All, CancellationToken cancellationToken = default);

	/// <summary>
	/// Get a range of entities by key
	/// </summary>
	Task<ICollection<TEntity>> GetRangeAsync(IEnumerable<TKey> keys, CacheOptions cacheOptions = CacheOptions.All, CancellationToken cancellationToken = default);
}