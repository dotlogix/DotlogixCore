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
        /// Get a single entity by guid
        /// </summary>
        public static ValueTask<TEntity> GetAsync<TEntity, TRepo>(this IUnitOfWorkContext context, object key) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity {
            var repository = context.UseRepository<TRepo>();
            return repository.GetAsync(key);
        }

        /// <summary>
        /// Get a range of entities by guid
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> GetRangeAsync<TEntity, TRepo>(this IUnitOfWorkContext context, IEnumerable<object> keys) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
	        var repository = context.UseRepository<TRepo>();
	        return repository.GetRangeAsync(keys);
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> GetAllAsync<TEntity, TRepo>(this IUnitOfWorkContext context) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity {
            var repository = context.UseRepository<TRepo>();
            return repository.GetAllAsync();
        }
        /// <summary>
        /// Get all entities matching an expression
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> FilterAllAsync<TEntity, TRepo>(this IUnitOfWorkContext context, Expression<Func<TEntity, bool>> filterExpression) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity {
            var repository = context.UseRepository<TRepo>();
            return repository.WhereAsync(filterExpression);
		}

        #endregion

		#region Remove
		/// <summary>
		/// Add a single entity to the set
		/// </summary>
		public static ValueTask<TEntity> AddAsync<TEntity, TRepo>(this IUnitOfWorkContext context, TEntity entity) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            var repository = context.UseRepository<TRepo>();
            return repository.AddAsync(entity);
            
        }
        /// <summary>
        /// Add a range of entities to the set
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> AddRangeAsync<TEntity, TRepo>(this IUnitOfWorkContext context, IEnumerable<TEntity> entities) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
            var repository = context.UseRepository<TRepo>();
            return repository.AddRangeAsync(entities);
        }

        #endregion


        #region Remove

        /// <summary>
        /// Queries the matching entity and remove it
        /// </summary>
        public static ValueTask<TEntity> RemoveAsync<TEntity, TRepo>(this IUnitOfWorkContext context, object key) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
	        var repository = context.UseRepository<TRepo>();
	        return repository.RemoveByKeyAsync(key);
        }

        /// <summary>
        /// Queries the matching entity and remove it
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> RemoveRangeAsync<TEntity, TRepo>(this IUnitOfWorkContext context, IEnumerable<object> keys) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
		{
			var repository = context.UseRepository<TRepo>();
			return repository.RemoveByKeyAsync(keys);
		}

        /// <summary>
        /// Queries the matching entities and remove them
        /// </summary>
        public static ValueTask<IEnumerable<TEntity>> RemoveWhereAsync<TEntity, TRepo>(this IUnitOfWorkContext context, Expression<Func<TEntity, bool>> filterExpression) where TRepo : IRepository<TEntity> where TEntity : class, ISimpleEntity
        {
	        var repository = context.UseRepository<TRepo>();
	        return repository.RemoveWhereAsync(filterExpression);
        }

        #endregion
    }
}
