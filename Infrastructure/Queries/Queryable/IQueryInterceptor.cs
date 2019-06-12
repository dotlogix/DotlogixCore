// ==================================================
// Copyright 2019(C) , DotLogix
// File:  IQueryInterceptor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  06.06.2019
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    /// <summary>
    ///     An interface to represent an interceptor of a query
    /// </summary>
    public interface IQueryInterceptor {
        /// <summary>
        ///     Executes a callback method before the query is executed
        /// </summary>
        IQuery<TValue> BeforeExecute<TValue>(IQuery<TValue> query);

        /// <summary>
        ///     Executes a callback method after the query is executed
        /// </summary>
        TResult AfterExecute<TResult>(TResult result);
    }
}
