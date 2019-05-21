// ==================================================
// Copyright 2019(C) , DotLogix
// File:  JsonWebTokenException.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  14.12.2018
// LastEdited:  03.01.2019
// ==================================================

#region
using System;
using System.Runtime.Serialization;
#endregion

namespace DotLogix.Core.Rest.Authentication.Jwt {
    public class JsonWebTokenException : Exception {
        public JsonWebTokenResult Result { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception"></see> class.</summary>
        public JsonWebTokenException(JsonWebTokenResult result) {
            Result = result;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception"></see> class with serialized data.</summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized
        ///     object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual
        ///     information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info">info</paramref> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        ///     The class name is null or
        ///     <see cref="P:System.Exception.HResult"></see> is zero (0).
        /// </exception>
        protected JsonWebTokenException(SerializationInfo info, StreamingContext context, JsonWebTokenResult result) : base(info, context) {
            Result = result;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception"></see> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public JsonWebTokenException(string message, JsonWebTokenResult result) : base(message) {
            Result = result;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Exception"></see> class with a specified error message
        ///     and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        public JsonWebTokenException(string message, Exception innerException, JsonWebTokenResult result) : base(message, innerException) {
            Result = result;
        }
    }
}
