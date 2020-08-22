using System;

namespace DotLogix.Core.Rest.Services.Results {
    public class WebServiceObjectResult : WebServiceResult, IWebServiceObjectResult {
        /// <inheritdoc />
        public Type ReturnType => ReturnValue.IsDefined ? ReturnValue.Value.GetType() : typeof(object);

        /// <inheritdoc />
        public Optional<object> ReturnValue { get; set; }
    }

    public class WebServiceObjectResult<T> : WebServiceResult, IWebServiceObjectResult<T> {
        /// <inheritdoc />
        public Type ReturnType => ReturnValue.IsDefined ? ReturnValue.Value.GetType() : typeof(T);

        /// <inheritdoc />
        public Optional<T> ReturnValue { get; set; }
        
        /// <inheritdoc />
        Optional<object> IWebServiceObjectResult.ReturnValue => ReturnValue.IsDefined
                                                                ? new Optional<object>(ReturnValue.Value)
                                                                : Optional<object>.Undefined;

        /// <summary>
        /// Converts a value to a corresponding object result
        /// </summary>
        public static implicit operator WebServiceObjectResult<T>(T value) {
            return new WebServiceObjectResult<T>{ReturnValue = value};
        }
    }
}