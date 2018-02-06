// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MimeType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Rest.Server.Http.Mime {
    public class MimeType {
        public string Code { get; }

        public MimeType(string code) {
            Code = code;
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
    }
}
