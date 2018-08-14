// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IQueryableQueryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Linq;
#endregion

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    public interface IQueryableQueryFactory {
        IQuery<T> CreateQuery<T>(IQueryable<T> queryable);
        IOrderedQuery<T> CreateQuery<T>(IOrderedQueryable<T> queryable);
        IQueryExecutor<T> CreateExecutor<T>(IQueryable<T> queryable);
    }
}
