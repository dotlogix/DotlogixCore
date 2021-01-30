// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfQueryableQueryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Queries {
    /// <summary>
    /// An implementation of the <see cref="IQueryableQueryFactory"/> for entity framework
    /// </summary>
    public class EfQueryableQueryFactory : QueryableQueryFactory
    {
        /// <summary>
        /// The static singleton instance
        /// </summary>
        public new static IQueryableQueryFactory Instance { get; } = new EfQueryableQueryFactory();

        /// <summary>
        /// Create a new instance of <see cref="EfQueryableQueryFactory"/>
        /// </summary>
        protected EfQueryableQueryFactory() { }

        /// <inheritdoc />
        public override IQueryExecutor<T> CreateExecutor<T>(IQuery<T> query) {
            return new InterceptableQueryExecutor<T>(query, q => new EfQueryExecutor<T>(q));
        }
    }
}
