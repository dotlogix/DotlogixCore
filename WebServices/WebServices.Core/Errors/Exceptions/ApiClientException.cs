using System;
using System.Collections.Generic;
using System.Net;

namespace DotLogix.WebServices.Core.Errors {
    public class ApiClientException : Exception {
        public ApiClientException(ApiError apiError) {
            ApiError = apiError;
        }

        public ApiError ApiError { get; }
        public string Kind => ApiError.Kind;
        public HttpStatusCode StatusCode => ApiError.StatusCode;
        public sealed override string Message => ApiError.Message;
        public IReadOnlyDictionary<string, object> Context => ApiError.Context;
    }
}