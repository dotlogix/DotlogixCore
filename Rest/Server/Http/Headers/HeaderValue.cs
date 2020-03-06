using System;
using System.Collections.Generic;
using System.Text;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Rest.Server.Http.Headers {
    public class HeaderValue {
        public HeaderValue(string value, IDictionary<string, Optional<string>> attributes = null) {
            Value = value;
            Attributes = attributes ?? new Dictionary<string, Optional<string>>();
        }

        public string Value { get; }
        public bool HasAttributes => Attributes.Count > 0;
        public IDictionary<string, Optional<string>> Attributes { get; }

        public Optional<string> GetAttribute(string name) {
            return Attributes.GetValue(name);
        }

        public bool HasAttribute(string name, bool onlyWithValue=false) {
            return Attributes.TryGetValue(name, out var value) && (value.IsDefined || onlyWithValue == false);
        }


        public override string ToString() {
            if(HasAttributes == false)
                return Value;

            var sb = new StringBuilder(Value);
            foreach(var attribute in Attributes) {
                sb.Append(';');
                sb.Append(attribute.Key);
                if(attribute.Value.IsDefined)
                    continue;

                sb.Append('=');
                sb.Append(attribute.Value);
            }
            return sb.ToString();
        }

        protected bool Equals(HeaderValue other) {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((HeaderValue)obj);
        }

        public override int GetHashCode() {
            return Value != null ? Value.GetHashCode() : 0;
        }

        /// <summary>
        ///     Returns a value that indicates whether the values of two
        ///     <see cref="T:DotLogix.Core.Rest.Server.Http.Mime.MimeType" /> objects are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(HeaderValue left, HeaderValue right) {
            return Equals(left, right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:DotLogix.Core.Rest.Server.Http.Mime.MimeType" />
        ///     objects have different values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(HeaderValue left, HeaderValue right) {
            return !Equals(left, right);
        }

        protected static (string code, IDictionary<string, Optional<string>> attributes) ExtractParts(string value) {
            var parts = value.Split(';');
            if(parts.Length < 1)
                throw new ArgumentException("The provided value is not a valid mime type", nameof(value));
            IDictionary<string, Optional<string>> attributes = null;
            if(parts.Length > 1) {
                attributes = new Dictionary<string, Optional<string>>(parts.Length - 1);
                for(var i = 1; i < parts.Length; i++) {
                    var keyValue = parts[i].Split(new[] {'='}, 2);
                    var attrName = keyValue[0].Trim(' ', '"');
                    var attrValue = keyValue.Length > 1 ? keyValue[1].Trim(' ', '"') : Optional<string>.Undefined;
                    attributes[attrName] = attrValue;
                }
            }

            var code = parts[0].Trim(' ');
            return (code, attributes);
        }
    }
}