using System;

namespace DotLogix.Core.Rest.Services {
    public class WebServiceObjectResult : WebServiceResult, IWebServiceObjectResult {
        /// <inheritdoc />
        public Type ReturnType => ReturnValue.IsDefined
                                  ? ReturnValue.Value.GetType()
                                  : null;

        /// <inheritdoc />
        public Optional<object> ReturnValue { get; set; }
    }

    public class WebServiceObjectResult<T> : WebServiceResult, IWebServiceObjectResult<T> {
        /// <inheritdoc />
        public Type ReturnType => typeof(T);

        /// <inheritdoc />
        public Optional<T> ReturnValue { get; set; }
        
        /// <inheritdoc />
        Optional<object> IWebServiceObjectResult.ReturnValue => ReturnValue.IsDefined
                                                                ? new Optional<object>(ReturnValue.Value)
                                                                : Optional<object>.Undefined;

        public static implicit operator WebServiceObjectResult<T>(T value) {
            return new WebServiceObjectResult<T>{ReturnValue = value};
        }
    }
}