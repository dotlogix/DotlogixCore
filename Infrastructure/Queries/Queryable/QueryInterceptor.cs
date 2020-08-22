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
        private readonly Func<IQueryInterceptionContext, bool> _beforeExecuteFunc;
        private readonly Func<IQueryInterceptionContext, bool> _afterExecuteFunc;
        private readonly Func<IQueryInterceptionContext, bool> _onErrorFunc;

        /// <inheritdoc />
        public QueryInterceptor(Func<IQueryInterceptionContext, bool> beforeExecuteFunc = null, Func<IQueryInterceptionContext, bool> afterExecuteFunc = null, Func<IQueryInterceptionContext, bool> onErrorFunc = null) {
            _beforeExecuteFunc = beforeExecuteFunc;
            _afterExecuteFunc = afterExecuteFunc;
            _onErrorFunc = onErrorFunc;
        }

        /// <inheritdoc />
        public bool BeforeExecute(IQueryInterceptionContext context) {
            return _beforeExecuteFunc?.Invoke(context) ?? true;
        }

        /// <inheritdoc />
        public bool AfterExecute(IQueryInterceptionContext context) {
            return _afterExecuteFunc?.Invoke(context) ?? true;
        }

        /// <inheritdoc />
        public bool HandleError(IQueryInterceptionContext context) {
            return _onErrorFunc?.Invoke(context) ?? true;
        }
    }
}