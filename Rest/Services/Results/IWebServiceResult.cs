using System;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;
using DotLogix.Core.Rest.Services.ResultWriters;

namespace DotLogix.Core.Rest.Services.Results {
    public interface IWebServiceResult {
        HttpStatusCode StatusCode { get; }
        MimeType ContentType { get; }
        IWebServiceResultWriter ResultWriter { get; }
        Optional<Exception> Exception { get; }
    }
}