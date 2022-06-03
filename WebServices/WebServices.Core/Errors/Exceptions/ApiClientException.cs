using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace DotLogix.WebServices.Core.Errors {
    public class ApiClientException : Exception {
        public ApiClientException(ApiError apiError, HttpResponseMessage response) {
            ApiError = apiError;
            Response = response;
        }

        public ApiError ApiError { get; }
        public HttpResponseMessage Response { get; }
        public string Kind => ApiError.Kind;
        public HttpStatusCode StatusCode => ApiError.StatusCode;
        public sealed override string Message => ApiError.Message;
        public IReadOnlyDictionary<string, object> Context => ApiError.Context;
    }
}