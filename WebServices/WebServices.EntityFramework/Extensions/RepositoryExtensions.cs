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
using DotLogix.WebServices.EntityFramework.Repositories;
#endregion

namespace DotLogix.WebServices.EntityFramework.Extensions; 

/// <summary>
/// A static class providing extension methods for <see cref="IRepository{TEntity, TKey}"/>
/// </summary>
public static class RepositoryExtensions {
    /// <summary>
    /// Queries the matching entities and remove them
    /// </summary>
    public static async Task<IEnumerable<TEntity>> RemoveWhereAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> filterExpression) where TEntity : class {
        var asyncResult = await repository.WhereAsync(filterExpression);

        if(asyncResult is not null)
            asyncResult = repository.RemoveRange(asyncResult);
            
        return asyncResult;
    }

    /// <summary>
    /// Queries the matching entity and remove it
    /// </summary>
    public static async Task<TEntity> RemoveByKeyAsync<TKey, TEntity>(this IRepository<TKey, TEntity> repository, TKey key) where TEntity : class {
        var asyncResult = await repository.GetAsync(key);
            
        if(asyncResult is not null)
            asyncResult = repository.Remove(asyncResult);
            
        return asyncResult;
    }

    /// <summary>
    /// Queries the matching entity and remove it
    /// </summary>
    public static async Task<IEnumerable<TEntity>> RemoveByKeyAsync<TKey, TEntity>(this IRepository<TKey, TEntity> repository, IEnumerable<TKey> keys) where TEntity : class {
        var asyncResult = await repository.GetRangeAsync(keys);

        if (asyncResult is not null)
            asyncResult = repository.RemoveRange(asyncResult);

        return asyncResult;
    }
}