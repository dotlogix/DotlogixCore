using System;
using DotLogix.Core;

namespace DotLogix.Architecture.Infrastructure.Queries {
    /// <summary>
    /// An interface representing the current context for query interceptors
    /// </summary>
    public interface IQueryInterceptionContext {
        /// <summary>
        /// The expected return type
        /// </summary>
        Type ResultType { get; }

        /// <summary>
        /// The query source type
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// The expected interface type of the query instance
        /// </summary>
        Type QueryType { get; }

        /// <summary>
        /// Gets or sets the query instance<br></br>
        /// Allows to modify the query before it is executed.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided value is not assignable to the query interface</exception>
        /// <exception cref="InvalidOperationException">Thrown if the property is set after query execution</exception>
        object Query { get; set; }

        /// <summary>
        /// Gets or sets the query result<br></br>
        /// If set before the query is executed, the execution is cancelled and the provided value is used as the result instead
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided value is not valid as query result</exception>
        Optional<object> Result { get; set; }

        /// <summary>
        /// Determines if query modification is supported
        /// </summary>
        bool AllowQueryModification { get; }
        /// <summary>
        /// Determines if the query is in a faulted state
        /// </summary>
        bool Faulted { get; }
        /// <summary>
        /// Determines if the query is in a success state
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Gets or sets the exception thrown while executing the query<br></br>
        /// If set before the query is executed, the execution is cancelled and the provided exception is thrown instead
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// Disables further query modification
        /// </summary>
        void DisableQueryModification();
    }
}