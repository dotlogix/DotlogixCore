using System;
using System.Linq.Expressions;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore.Query;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    /// <summary>
    /// A class to represent entity query event args
    /// </summary>
    public class QueryResultEventArgs : QueryEventArgs {
        public QueryContext QueryContext { get; }
        public Expression Expression { get; }

        public bool HasResult { get; private set; }
        public object Result { get; private set; }
        public Exception Exception { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="QueryCompileEventArgs"/>
        /// </summary>
        public QueryResultEventArgs(QueryContext queryContext, Expression expression, Type resultType) : base(resultType) {
            QueryContext = queryContext;
            Expression = expression;
        }
        
        public void SetResult(object result) {
            var isValid = result != null ? ResultType.IsInstanceOfType(result) : ResultType.IsNullable();
            if(isValid == false) {
                throw new ArgumentException($"Can not convert result type {result?.GetType().GetFriendlyGenericName()} to target return type {ResultType.GetFriendlyGenericName()}");
            }

            Result = result;
            Exception = null;
            HasResult = true;
        }
        public void SetException(Exception exception) {
            Result = null;
            Exception = exception;
            HasResult = true;
        }
        
        public void Reset() {
            Result = null;
            Exception = null;
            HasResult = false;
        }
    }
}