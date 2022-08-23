using System.Net;

namespace DotLogix.WebServices.Core.Errors; 

public class UnauthorizedApiException : StatusCodeApiException {
    public UnauthorizedApiException(string message = null) : base(message, HttpStatusCode.Unauthorized) {
    }
}