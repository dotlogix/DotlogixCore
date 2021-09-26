// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntitySet.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Infrastructure {
    

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
        Task<ICollection<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        #endregion

        #region Remove

        /// <summary>
        /// Remove a single entity from the set
        /// </summary>
        Task<TEntity> RemoveAsync(TEntity entity);

        /// <summary>
        /// Remove a range of entities from the set
        /// </summary>
        Task<ICollection<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities);

        #endregion

        #region Attach

        /// <summary>
        /// Attach a single entity to the underlying change tracker
        /// </summary>
        TEntity Attach(TEntity entity);

        /// <summary>
        /// Attach a range of entities to the underlying change tracker
        /// </summary>
        ICollection<TEntity> AttachRange(IEnumerable<TEntity> entities);

        #endregion
        
        #region Query
        /// <summary>
        /// Create a linq style query to allow more advanced requests to the entity set
        /// </summary>
        IQueryable<TEntity> Query();
        #endregion
    }
}
