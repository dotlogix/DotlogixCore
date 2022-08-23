using System.Net;

namespace DotLogix.WebServices.Core.Errors; 

public class ServiceUnavailableApiException : StatusCodeApiException {
    public ServiceUnavailableApiException(string message = null) : base(message, HttpStatusCode.ServiceUnavailable) {
    }
}