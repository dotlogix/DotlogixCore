using System;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;
using DotLogix.Core.Rest.Services.ResultWriters;

namespace DotLogix.Core.Rest.Services.Results {
    public class WebServiceResult : IWebServiceResult {
        public HttpStatusCode StatusCode { get; set; }
        public MimeType ContentType { get; set; }
        public IWebServiceResultWriter ResultWriter { get; set; }

        /// <inheritdoc />
        public Optional<Exception> Exception { get; set; }

        /// <inheritdoc />
        public WebServiceResult(HttpStatusCode statusCode = null, MimeType contentType = null) {
            StatusCode = statusCode;
            ContentType = contentType;
        }

        /// <summary>
        /// Converts a value to a corresponding object result
        /// </summary>
        public static implicit operator WebServiceResult(HttpStatusCode statusCode) {
            return new WebServiceResult(statusCode);
        }
    }
}