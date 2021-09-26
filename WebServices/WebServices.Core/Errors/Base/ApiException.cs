using System.Net;

namespace DotLogix.WebServices.Core.Errors {
    public abstract class ApiException : ApiExceptionBase {
        public new string Message { get; set; }

        protected ApiException(string kind, string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(kind, statusCode) {
            Message = message;
        }
        
        protected ApiException(string kind, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(kind, statusCode) {
        }

        protected override string GetErrorMessage() {
            return Message;
        }
    }
}
