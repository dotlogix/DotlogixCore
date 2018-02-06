// ==================================================
// Copyright 2018(C) , DotLogix
// File:  HttpMethodAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Http {
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpMethodAttribute : Attribute {
        public HttpMethods Methods { get; }

        public HttpMethodAttribute(HttpMethods methods) {
            Methods = methods;
        }
    }
}
