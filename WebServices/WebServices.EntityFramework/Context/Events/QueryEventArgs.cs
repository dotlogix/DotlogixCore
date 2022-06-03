using System;

namespace DotLogix.WebServices.EntityFramework.Context.Events {
    public class QueryEventArgs : EventArgs {
        public Type ResultType { get; }

        public QueryEventArgs(Type resultType) {
            ResultType = resultType;
        }
    }
}