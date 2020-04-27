// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonRouteResultWriterAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Services.Attributes;
#endregion

namespace DotLogix.Core.Rest.Json {
    public class JsonRouteResultWriterAttribute : RouteResultWriterAttribute {
        public JsonRouteResultWriterAttribute() : base(() => JsonNodesResultWriter.Instance) { }
    }
}
