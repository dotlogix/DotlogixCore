// ==================================================
// Copyright 2018(C) , DotLogix
// File:  UnitOfWorkExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  09.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Repositories;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    /// <summary>
    /// A static class providing extension methods for <see cref="IUnitOfWork"/>
    /// </summary>
    public static class UnitOfWorkExtensions {
        #region Get
        /// <summary>
        /// Get a single entity by key
        /// </summary>
        public static ValueTask<TEntity> GetAsync<TKey, TEntity>(this IUnitOfWorkContext context, TKey key) where TEntity : class, new() {
            var repository = context.UseRepository<IRepository<TKey, TEntity>>();
            return repository.GetAsync(key);
        }

        /// <summary>
        /// Get a range of entities by key
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> GetRangeAsync<TKey, TEntity>(this IUnitOfWorkContext context, IEnumerable<TKey> keys) where TEntity : class, new() {
	        var repository = context.UseRepository<IRepository<TKey, TEntity>>();
	        return repository.GetRangeAsync(keys);
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> GetAllAsync<TEntity>(this IUnitOfWorkContext context) where TEntity : class, new() {
            var repository = context.UseRepository<IRepository<TEntity>>();
            return repository.GetAllAsync();
        }
        /// <summary>
        /// Get all entities matching an expression
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> WhereAsync<TEntity>(this IUnitOfWorkContext context, Expression<Func<TEntity, bool>> filterExpression) where TEntity : class, new() {
            var repository = context.UseRepository<IRepository<TEntity>>();
            return repository.WhereAsync(filterExpression);
		}

        #endregion

		#region Remove
		/// <summary>
		/// Add a single entity to the set
		/// </summary>
		public static ValueTask<TEntity> AddAsync<TEntity>(this IUnitOfWorkContext context, TEntity entity) where TEntity : class, new() {
            var repository = context.UseRepository<IRepository<TEntity>>();
            return repository.AddAsync(entity);
            
        }
        /// <summary>
        /// Add a range of entities to the set
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> AddRangeAsync<TEntity>(this IUnitOfWorkContext context, IEnumerable<TEntity> entities) where TEntity : class, new() {
            var repository = context.UseRepository<IRepository<TEntity>>();
            return repository.AddRangeAsync(entities);
        }

        #endregion


        #region Remove

        /// <summary>
        /// Queries the matching entity and remove it
        /// </summary>
        public static ValueTask<TEntity> RemoveAsync<TKey, TEntity>(this IUnitOfWorkContext context, TKey key) where TEntity : class, ISimpleEntity, new() {
	        var repository = context.UseRepository<IRepository<TKey, TEntity>>();
	        return repository.RemoveByKeyAsync(key);
        }

        /// <summary>
        /// Queries the matching entity and remove it
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> RemoveRangeAsync<TKey, TEntity>(this IUnitOfWorkContext context, IEnumerable<TKey> keys) where TEntity : class, new() {
			var repository = context.UseRepository<IRepository<TKey, TEntity>>();
			return repository.RemoveByKeyAsync(keys);
		}

        /// <summary>
        /// Queries the matching entities and remove them
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> RemoveWhereAsync<TEntity>(this IUnitOfWorkContext context, Expression<Func<TEntity, bool>> filterExpression) where TEntity : class, ISimpleEntity, new() {
	        var repository = context.UseRepository<IRepository<TEntity>>();
	        return repository.RemoveWhereAsync(filterExpression);
        }

        #endregion
    }
}
