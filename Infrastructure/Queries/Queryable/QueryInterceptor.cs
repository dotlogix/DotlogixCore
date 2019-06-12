// ==================================================
// Copyright 2019(C) , DotLogix
// File:  QueryInterceptor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  ..
// LastEdited:  06.06.2019
// ==================================================

using System;

namespace DotLogix.Architecture.Infrastructure.Queries.Queryable {
    /// <summary>
    /// An interceptor for <see cref="IQuery{T}"/>
    /// </summary>
    public class QueryInterceptor : IQueryInterceptor {
        private Func<object, object> BeforeExecuteFunc { get; }
        private Func<object, object> AfterExecuteFunc { get; }

        /// <summary>
        /// Creates a new instance of <see cref="QueryInterceptor"/>
        /// </summary>
        public QueryInterceptor(Func<object, object> beforeExecuteFunc, Func<object, object> afterExecuteFunc) {
            BeforeExecuteFunc = beforeExecuteFunc;
            AfterExecuteFunc = afterExecuteFunc;
        }

        /// <inheritdoc />
        public IQuery<TValue> BeforeExecute<TValue>(IQuery<TValue> query) {
            return BeforeExecuteFunc != null ? (IQuery<TValue>)BeforeExecuteFunc(query) : query;
        }

        /// <inheritdoc />
        public TResult AfterExecute<TResult>(TResult result) {
            return AfterExecuteFunc != null ? (TResult)AfterExecuteFunc(result) : result;
        }
    }
}