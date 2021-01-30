// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntitySet.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Repositories;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {

    /// <summary>
    /// An interface for entity sets
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntitySet<TEntity> where TEntity : class, new() {
        /// <summary>
        /// The current entity context
        /// </summary>
        IEntityContext EntityContext { get; }
        
        #region Add

        /// <summary>
        /// Add a single entity to the set
        /// </summary>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Add a range of entities to the set
        /// </summary>
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        #endregion

        #region Remove

        /// <summary>
        /// Remove a single entity from the set
        /// </summary>
        Task<TEntity> RemoveAsync(TEntity entity);

        /// <summary>
        /// Remove a range of entities from the set
        /// </summary>
        Task<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities);

        #endregion

        #region ReAttach

        /// <summary>
        /// Reattach a single entity to the underlying change tracker
        /// </summary>
        Task<TEntity> ReAttachAsync(TEntity entity);

        /// <summary>
        /// Reattach a range of entities to the underlying change tracker
        /// </summary>
        Task<IEnumerable<TEntity>> ReAttachRangeAsync(IEnumerable<TEntity> entities);

        #endregion

        #region Query
        /// <summary>
        /// Create a linq style query to allow more advanced requests to the entity set
        /// </summary>
        IQuery<TEntity> Query();

        /// <summary>
        /// Create a linq style query to allow more advanced requests to the entity set
        /// </summary>
        IQuery<TEntity> Query(params IQueryModifier<TEntity>[] filters);
        #endregion
    }
}
