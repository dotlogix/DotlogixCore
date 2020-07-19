// ==================================================
// Copyright 2018(C) , DotLogix
// File:  OrderedQueryableQuery.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries {
    /// <summary>
    /// An extension of the <see cref="QueryableQuery{TValue}"/> to allow multi level ordering
    /// </summary>
    public class OrderedQueryableQuery<TSource> : QueryableQuery<TSource>, IOrderedQuery<TSource> {
        private readonly IOrderedQueryable<TSource> _innerQueryable;

        /// <summary>
        /// Creates a new instance of <see cref="OrderedQueryableQuery{TSource}"/>
        /// </summary>
        public OrderedQueryableQuery(IOrderedQueryable<TSource> innerQueryable, IQueryableQueryFactory factory, IEnumerable<IQueryInterceptor> interceptors) : base(innerQueryable, factory, interceptors) {
            _innerQueryable = innerQueryable;
        }

        #region ThenBy
        /// <inheritdoc />
        public IOrderedQuery<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector) {
            return CreateQuery(_innerQueryable.ThenBy(keySelector));
        }

        /// <inheritdoc />
        public IOrderedQuery<TSource> ThenByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector) {
            return CreateQuery(_innerQueryable.ThenByDescending(keySelector));
        }

        #endregion
    }
}
