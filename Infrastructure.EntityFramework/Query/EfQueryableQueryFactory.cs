// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfQueryableQueryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Linq;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Query {
    public class EfQueryableQueryFactory : IQueryableQueryFactory {
        public static IQueryableQueryFactory Instance { get; } = new EfQueryableQueryFactory();
        private EfQueryableQueryFactory() { }

        public IQuery<T> CreateQuery<T>(IQueryable<T> queryable) {
            return new QueryableQuery<T>(queryable, this);
        }

        public IOrderedQuery<T> CreateQuery<T>(IOrderedQueryable<T> queryable) {
            return new OrderedQueryableQuery<T>(queryable, this);
        }

        public IQueryExecutor<T> CreateExecutor<T>(IQueryable<T> queryable) {
            return new EfQueryExecutor<T>(queryable);
        }
    }
}
