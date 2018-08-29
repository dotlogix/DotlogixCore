// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonRouteResultWriterAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.ResultWriter {
    public class JsonRouteResultWriterAttribute : RouteResultWriterAttribute {
        public JsonRouteResultWriterAttribute() : base(()=> WebRequestResultJsonWriter.Instance) { }
    }
}
