// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RestException.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Server.Http.State;
#endregion

namespace DotLogix.Core.Rest.Services.Exceptions {
    public class RestException : Exception {
        public HttpStatusCode ErrorCode { get; set; }

        public RestException(HttpStatusCode errorCode) {
            ErrorCode = errorCode;
        }

        public RestException(HttpStatusCode errorCode, string message) : base(message) {
            ErrorCode = errorCode;
        }

        public RestException(HttpStatusCode errorCode, string message, Exception innerException) :
            base(message, innerException) {
            ErrorCode = errorCode;
        }
    }
}
