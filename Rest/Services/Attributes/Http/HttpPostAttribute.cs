// ==================================================
// Copyright 2018(C) , DotLogix
// File:  HttpPostAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Server.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Http {
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : HttpMethodAttribute {
        public HttpPostAttribute() : base(HttpMethods.Post) { }
    }
}
