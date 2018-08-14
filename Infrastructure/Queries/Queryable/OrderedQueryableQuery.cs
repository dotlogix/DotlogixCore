// ==================================================
// Copyright 2018(C) , DotLogix
// File:  OrderedQueryableQuery.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Linq;
using System.Linq.Expressions;
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public class OrderedQueryableQuery<TSource> : QueryableQuery<TSource>, IOrderedQuery<TSource> {
        private readonly IOrderedQueryable<TSource> _innerQueryable;

        public OrderedQueryableQuery(IOrderedQueryable<TSource> innerQueryable, IQueryableQueryFactory factory) : base(innerQueryable, factory) {
            _innerQueryable = innerQueryable;
        }

        #region ThenBy

        public IOrderedQuery<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector) {
            return CreateQuery(_innerQueryable.ThenBy(keySelector));
        }

        public IOrderedQuery<TSource> ThenByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector) {
            return CreateQuery(_innerQueryable.ThenByDescending(keySelector));
        }

        #endregion
    }
}
