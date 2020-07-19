using System;
using DotLogix.Core;

namespace DotLogix.Architecture.Infrastructure.Queries {
    /// <summary>
    /// An implementation of the <see cref="IQueryInterceptionContext"/>
    /// </summary>
    public sealed class QueryInterceptionContext : IQueryInterceptionContext {
        private bool _allowQueryModification;

        /// <inheritdoc />
        public Type ResultType { get; }

        /// <inheritdoc />
        public Type SourceType { get; }

        /// <inheritdoc />
        public Type QueryType { get; }

        /// <inheritdoc />
        private QueryInterceptionContext(Type sourceType, Type resultType, bool allowQueryModification) {
            _allowQueryModification = allowQueryModification;
            ResultType = resultType ?? throw new ArgumentNullException(nameof(resultType));
            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            QueryType = typeof(IQuery<>).MakeGenericType(sourceType);
        }

        private object _query;

        /// <inheritdoc />
        public object Query {
            get => _query;
            set {
                if(_allowQueryModification == false)
                    throw new InvalidOperationException($"The type {value?.GetType().Name ?? "null"} is not assignable to query interface {QueryType.Name}");

                if(QueryType.IsInstanceOfType(value) == false)
                    throw new ArgumentException($"The type {value?.GetType().Name ?? "null"} is not assignable to query interface {QueryType.Name}");

                _query = value;
            }
        }

        private Optional<object> _result;

        /// <inheritdoc />
        public Optional<object> Result {
            get => _result;
            set {
                if (value.IsDefined && ((value.Value == null && ResultType.IsClass == false) || (value.Value != null && ResultType.IsInstanceOfType(value.Value) == false)))
                    throw new ArgumentException($"The type {value.Value?.GetType().Name ?? "null"} is not assignable to result type {ResultType.Name}");
                _result = value;
            }
        }

        /// <inheritdoc />
        public bool AllowQueryModification => _allowQueryModification;

        /// <inheritdoc />
        public bool Faulted => Exception != null;

        /// <inheritdoc />
        public bool Success => Result.IsDefined && Exception == null;

        /// <inheritdoc />
        public Exception Exception { get; set; }
        
        /// <inheritdoc />
        public void DisableQueryModification() {
            _allowQueryModification = false;
        }

        /// <summary>
        /// Creates a new instance of a query interception context
        /// </summary>
        public static IQueryInterceptionContext FromQuery<T, TResult>(IQuery<T> query) {
            return new QueryInterceptionContext(typeof(T), typeof(TResult), true) {
                                                                                  _query = query,
                                                                                  };
        }
    }
}