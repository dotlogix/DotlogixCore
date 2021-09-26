using System.Net;

namespace DotLogix.WebServices.Core.Errors {
    public static class ApiErrorKinds {
        #region Custom
        
        public const string Validation = "Validation";
        public const string PropertyConflict = "PropertyConflict";
        public const string FilterNotFound = "FilterNotFound";
        public const string KeyNotFound = "KeyNotFound";
        #endregion


        #region Http
        
        public const string TooManyRequests = nameof(HttpStatusCode.TooManyRequests);
        public const string InternalServerError = nameof(HttpStatusCode.InternalServerError);
        public const string ServiceUnavailable = nameof(HttpStatusCode.ServiceUnavailable);
        public const string Unauthorized = nameof(HttpStatusCode.Unauthorized);
        public const string Conflict = nameof(HttpStatusCode.Conflict);
        public const string Forbidden = nameof(HttpStatusCode.Forbidden);
        public const string BadRequest = nameof(HttpStatusCode.BadRequest);
        #endregion
    }
}
