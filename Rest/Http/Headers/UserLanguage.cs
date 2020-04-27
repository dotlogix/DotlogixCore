using System.Collections.Generic;
using System.Globalization;

namespace DotLogix.Core.Rest.Http.Headers {
    public class UserLanguage : HeaderValue {
        public UserLanguage(string value, IDictionary<string, Optional<string>> attributes = null) : base(value, attributes) { }
        
        public CultureInfo CultureInfo {
            get {
                try {
                    return CultureInfo.GetCultureInfo(Value);
                } catch {
                    return null;
                }
            }
        }

        public static UserLanguage Parse(string value) {
            if(value == null)
                return new UserLanguage(null);
            var parts = ExtractParts(value);
            return new UserLanguage(parts.code, parts.attributes);
        }
    }
}