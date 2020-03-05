// ==================================================
// Copyright 2019(C) , DotLogix
// File:  IQueryInterceptor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  06.06.2019
// ==================================================

using System.Threading.Tasks;
using DotLogix.Core.Extensions;

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    /// <summary>
    ///     An interface to represent an interceptor of a query
    /// </summary>
    public interface IQueryInterceptor {
        /// <summary>
        ///     Executes a callback method before the query is executed
        /// </summary>
        /// <returns>true to continue execution of the other interceptors, otherwise false</returns>
        bool BeforeExecute(IQueryInterceptionContext context);

        /// <summary>
        ///     Executes a callback method after the query is executed
        /// </summary>
        /// <returns>true to continue execution of the other interceptors, otherwise false</returns>
        bool AfterExecute(IQueryInterceptionContext context);

        /// <summary>
        ///     Executes a callback method on error optionally returning a fallback result
        /// </summary>
        /// <returns>true to continue execution of the other interceptors, otherwise false</returns>
        bool HandleError(IQueryInterceptionContext context);
    }
}
