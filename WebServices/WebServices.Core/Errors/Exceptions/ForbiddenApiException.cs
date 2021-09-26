using System.Net;

namespace DotLogix.WebServices.Core.Errors {
    public class ForbiddenApiException : StatusCodeApiException {
        public ForbiddenApiException(string message = null) : base(message, HttpStatusCode.Forbidden) {
        }
    }
}