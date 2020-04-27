using System;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;

namespace DotLogix.Core.Rest.Services {
    public interface IWebServiceResult {
        HttpStatusCode StatusCode { get; }
        MimeType ContentType { get; }
        IWebServiceResultWriter ResultWriter { get; }
        Optional<Exception> Exception { get; }
    }
}