using System;

namespace DotLogix.Infrastructure.EntityFramework.Hooks {
    public class QueryEventArgs : EventArgs {
        public Type ResultType { get; }

        public QueryEventArgs(Type resultType) {
            ResultType = resultType;
        }
    }
}