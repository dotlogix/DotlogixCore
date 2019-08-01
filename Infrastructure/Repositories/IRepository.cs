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
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    /// <summary>
    /// A non generic marker interface for repository types
    /// </summary>
    public interface IRepository { }

    /// <summary>
    /// An interface to represent a repository
    /// </summary>
    public interface IRepository<TEntity> : IRepository where TEntity : class, ISimpleEntity {
        #region Get
        /// <summary>
        /// Get a single entity by id
        /// </summary>
        Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Get a range of entities by id
        /// </summary>
        Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// Get a single entity by guid
        /// </summary>
        Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default);
        /// <summary>
        /// Get a range of entities by guid
        /// </summary>
        Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default);
        /// <summary>
        /// Get all entities
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all entities matching an expression
        /// </summary>
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default);
        #endregion

        #region Add
        /// <summary>
        /// Add a single entity to the set
        /// </summary>
        void Add(TEntity entity);
        /// <summary>
        /// Add a range of entities to the set
        /// </summary>
        void AddRange(IEnumerable<TEntity> entities);
        #endregion

        #region Remove
        /// <summary>
        /// Remove a single entity from the set
        /// </summary>
        void Remove(TEntity entity);
        /// <summary>
        /// Remove a range of entities from the set
        /// </summary>
        void RemoveRange(IEnumerable<TEntity> entities);
        #endregion
    }
}
