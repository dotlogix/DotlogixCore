using System.Net;

namespace DotLogix.WebServices.Core.Errors; 

public class BadRequestApiException : StatusCodeApiException {
    public BadRequestApiException(string message) : base(message, HttpStatusCode.BadRequest) {
    }
}