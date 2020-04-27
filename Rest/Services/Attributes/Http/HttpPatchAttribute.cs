// ==================================================
// Copyright 2018(C) , DotLogix
// File:  HttpPutAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Http;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPatchAttribute : HttpMethodAttribute {
        public HttpPatchAttribute() : base(HttpMethods.Patch) { }
    }
}
