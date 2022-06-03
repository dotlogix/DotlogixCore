using System.Net;

namespace DotLogix.WebServices.Core.Errors {
    public static class ApiErrorKinds {
        #region Custom
        
        public const string Validation = "Validation";
        public const string Conflict = "Conflict";
        public const string PropertyConflict = "PropertyConflict";
        public const string FilterNotFound = "FilterNotFound";
        public const string KeyNotFound = "KeyNotFound";
        #endregion
    }
}
