// ==================================================
// Copyright 2018(C) , DotLogix
// File:  HttpStatusCode.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Server.Http.State {
    public class HttpStatusCode {
        public HttpStatusCodeGroup Group => HttpStatusCodes.GetGroup(Code);
        public int Code { get; }
        public string Description { get; }

        public HttpStatusCode(int code, string description) {
            Code = code;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        protected bool Equals(HttpStatusCode other) {
            return Code == other.Code;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return $"{Code} {Description}";
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((HttpStatusCode)obj);
        }

        public override int GetHashCode() {
            return Code;
        }

        /// <summary>
        ///     Returns a value that indicates whether the values of two
        ///     <see cref="T:DotLogix.Core.Rest.Server.Http.State.HttpStatusCode" /> objects are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(HttpStatusCode left, HttpStatusCode right) {
            return Equals(left, right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two
        ///     <see cref="T:DotLogix.Core.Rest.Server.Http.State.HttpStatusCode" /> objects have different values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(HttpStatusCode left, HttpStatusCode right) {
            return !Equals(left, right);
        }
    }
}
