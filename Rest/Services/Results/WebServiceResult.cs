using System;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;

namespace DotLogix.Core.Rest.Services {
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
    }
}