// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepositoryExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    /// <summary>
    /// A static class providing extension methods for <see cref="IRepository{TEntity, TKey}"/>
    /// </summary>
    public static class RepositoryExtensions {
        /// <summary>
        /// Queries the matching entities and remove them
        /// </summary>
        public static async ValueTask<IEnumerable<TEntity>> RemoveWhereAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filterExpression) where TEntity : class, new() {
            var asyncResult = await repository.WhereAsync(filterExpression);

            if(asyncResult != null)
	            asyncResult = await repository.RemoveRangeAsync(asyncResult);
            
            return asyncResult;
		}

        /// <summary>
        /// Queries the matching entity and remove it
        /// </summary>
        public static async ValueTask<TEntity> RemoveByKeyAsync<TKey, TEntity>(this IRepository<TKey, TEntity> repository, TKey key) where TEntity : class, new() {
	        var asyncResult = await repository.GetAsync(key);
            
            if(asyncResult != null)
		        asyncResult = await repository.RemoveAsync(asyncResult);
            
            return asyncResult;
		}

        /// <summary>
        /// Queries the matching entity and remove it
        /// </summary>
        public static async ValueTask<IEnumerable<TEntity>> RemoveByKeyAsync<TKey, TEntity>(this IRepository<TKey, TEntity> repository, IEnumerable<TKey> keys) where TEntity : class, new() {
            var asyncResult = await repository.GetRangeAsync(keys);

            if (asyncResult != null)
                asyncResult = await repository.RemoveRangeAsync(asyncResult);

            return asyncResult;
        }
    }
}
