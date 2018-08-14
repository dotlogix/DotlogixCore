// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MimeType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Mime {
    public class MimeType {
        public string Code { get; }
        public bool HasAttributes => (Attributes != null) && (Attributes.Count > 0);
        public IDictionary<string, Optional<string>> Attributes { get; }

        public MimeType(string code, IDictionary<string, Optional<string>> attributes = null) {
            Code = code;
            Attributes = attributes != null ? new ReadOnlyDictionary<string, Optional<string>>(attributes) : null;
        }

        public override string ToString() {
            if(HasAttributes == false)
                return Code;

            var sb = new StringBuilder(Code);
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

        protected bool Equals(MimeType other) {
            return string.Equals(Code, other.Code);
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((MimeType)obj);
        }

        public override int GetHashCode() {
            return Code != null ? Code.GetHashCode() : 0;
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
        public static bool operator ==(MimeType left, MimeType right) {
            return Equals(left, right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:DotLogix.Core.Rest.Server.Http.Mime.MimeType" />
        ///     objects have different values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(MimeType left, MimeType right) {
            return !Equals(left, right);
        }

        public static MimeType Parse(string value) {
            if(value == null)
                return new MimeType(null);

            var parts = value.Split(';');
            if(parts.Length < 1)
                throw new ArgumentException("The provided value is not a valid mime type", nameof(value));
            IDictionary<string, Optional<string>> attributes = null;
            if(parts.Length > 1) {
                attributes = new Dictionary<string, Optional<string>>(parts.Length - 1);
                for(var i = 1; i < parts.Length; i++) {
                    var keyValue = parts[i].Split(new[] {'='}, 2);
                    var attrName = keyValue[0].Trim(' ');
                    var attrValue = keyValue.Length > 1 ? keyValue[1].Trim(' ') : Optional<string>.Undefined;
                    attributes[attrName] = attrValue;
                }
            }
            var code = parts[0].Trim(' ');
            return new MimeType(code, attributes);
        }
    }
}
