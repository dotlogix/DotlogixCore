// ==================================================
// Copyright 2018(C) , DotLogix
// File:  QueryableQueryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Linq;
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public class QueryableQueryFactory : IQueryableQueryFactory {
        public static IQueryableQueryFactory Instance { get; } = new QueryableQueryFactory();
        private QueryableQueryFactory() { }

        public IQuery<T> CreateQuery<T>(IQueryable<T> queryable) {
            return new QueryableQuery<T>(queryable, this);
        }

        public IOrderedQuery<T> CreateQuery<T>(IOrderedQueryable<T> queryable) {
            return new OrderedQueryableQuery<T>(queryable, this);
        }

        public IQueryExecutor<T> CreateExecutor<T>(IQueryable<T> queryable) {
            return new QueryableQueryExecutor<T>(queryable);
        }
    }
}
