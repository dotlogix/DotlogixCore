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
    public class EfQueryableQueryFactory : QueryableQueryFactory
    {
        public new static IQueryableQueryFactory Instance { get; } = new EfQueryableQueryFactory();
        protected EfQueryableQueryFactory() { }

        public override IQueryExecutor<T> CreateExecutor<T>(IQuery<T> query) {
            return new InterceptableQueryExecutor<T>(query, q => new EfQueryExecutor<T>(q));
        }
    }
}
