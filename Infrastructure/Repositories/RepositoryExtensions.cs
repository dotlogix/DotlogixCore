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
using DotLogix.Architecture.Infrastructure.Entities;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories {
    /// <summary>
    /// A static class providing extension methods for <see cref="IRepository{TEntity}"/>
    /// </summary>
    public static class RepositoryExtensions {
        /// <summary>
        /// Queries the matching entities and remove them
        /// </summary>
        public static async ValueTask<IEnumerable<TEntity>> RemoveWhereAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filterExpression) where TEntity : class, ISimpleEntity {
            var asyncResult = repository.WhereAsync(filterExpression);
            if(!asyncResult.IsCompletedSuccessfully)
	            await asyncResult;

            if(asyncResult.Result != null)
	            asyncResult = repository.RemoveRangeAsync(asyncResult.Result);

            if(!asyncResult.IsCompletedSuccessfully)
	            await asyncResult;

            return asyncResult.Result;
		}

        /// <summary>
        /// Queries the matching entity and remove it
        /// </summary>
        public static async ValueTask<TEntity> RemoveByKeyAsync<TEntity>(this IRepository<TEntity> repository, object key) where TEntity : class, ISimpleEntity
        {
	        var asyncResult = repository.GetAsync(key);
	        if(!asyncResult.IsCompletedSuccessfully)
		        await asyncResult;

	        if(asyncResult.Result != null)
		        asyncResult = repository.RemoveAsync(asyncResult.Result);

	        if(!asyncResult.IsCompletedSuccessfully)
		        await asyncResult;

	        return asyncResult.Result;
		}

        /// <summary>
        /// Queries the matching entity and remove it
        /// </summary>
        public static async ValueTask<IEnumerable<TEntity>> RemoveByKeyAsync<TEntity>(this IRepository<TEntity> repository, IEnumerable<object> keys) where TEntity : class, ISimpleEntity
        {
	        var asyncResult = repository.GetRangeAsync(keys);
	        if(!asyncResult.IsCompletedSuccessfully)
		        await asyncResult;

	        if(asyncResult.Result != null)
		        asyncResult = repository.RemoveRangeAsync(asyncResult.Result);

	        if(!asyncResult.IsCompletedSuccessfully)
		        await asyncResult;

	        return asyncResult.Result;
        }
	}
}
