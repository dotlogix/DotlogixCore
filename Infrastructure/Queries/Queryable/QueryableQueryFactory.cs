// ==================================================
// Copyright 2018(C) , DotLogix
// File:  QueryableQueryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public class QueryableQueryFactory : IQueryableQueryFactory {
        public static IQueryableQueryFactory Instance { get; } = new QueryableQueryFactory();
        protected QueryableQueryFactory() { }

        public virtual IQuery<T> CreateQuery<T>(IQueryable<T> queryable, IEnumerable<IQueryInterceptor> interceptors) {
            return new QueryableQuery<T>(queryable, this, interceptors);
        }

        public virtual IOrderedQuery<T> CreateQuery<T>(IOrderedQueryable<T> queryable, IEnumerable<IQueryInterceptor> interceptors) {
            return new OrderedQueryableQuery<T>(queryable, this, interceptors);
        }

        public virtual IQueryExecutor<T> CreateExecutor<T>(IQuery<T> query) {
            return new InterceptableQueryExecutor<T>(query, q => new QueryableQueryExecutor<T>(q));
        }
    }
}
