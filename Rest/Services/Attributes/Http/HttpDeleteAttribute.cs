// ==================================================
// Copyright 2018(C) , DotLogix
// File:  HttpDeleteAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Http {
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : HttpMethodAttribute {
        public HttpDeleteAttribute() : base(HttpMethods.Delete) { }
    }
}
