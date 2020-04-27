using System.Collections.Generic;

namespace DotLogix.Core.Rest.Http.Headers {
    public class ContentDisposition : HeaderValue {
        public ContentDisposition(string value, IDictionary<string, Optional<string>> attributes = null) : base(value, attributes) { }

        public static ContentDisposition Parse(string value) {
            if(value == null)
                return new ContentDisposition(null);
            var parts = ExtractParts(value);
            return new ContentDisposition(parts.code, parts.attributes);
        }
    }
}