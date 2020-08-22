// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IQueryableQueryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    /// <summary>
    /// An interface to represent a factory creating objects used for <see cref="IQuery{T}"/>
    /// </summary>
    public interface IQueryableQueryFactory {
        /// <summary>
        /// Create a new query based on an <see cref="IQueryable{T}"/>
        /// </summary>
        IQuery<T>  CreateQuery<T>(IQueryable<T> queryable, IEnumerable<IQueryInterceptor> interceptors = null);
        /// <summary>
        /// Create a new query based on an <see cref="IOrderedQuery{T}"/>
        /// </summary>
        IOrderedQuery<T> CreateQuery<T>(IOrderedQueryable<T> queryable, IEnumerable<IQueryInterceptor> interceptors = null);
        /// <summary>
        /// Create a new query executor for a <see cref="IQuery{T}"/>
        /// </summary>
        IQueryExecutor<T> CreateExecutor<T>(IQuery<T> query);
    }
}
