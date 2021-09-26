using System.Net;

namespace DotLogix.WebServices.Core.Errors {
    public class StatusCodeApiException : ApiException {
        public StatusCodeApiException(string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(statusCode.ToString(), message, statusCode) {
        }
    }
}