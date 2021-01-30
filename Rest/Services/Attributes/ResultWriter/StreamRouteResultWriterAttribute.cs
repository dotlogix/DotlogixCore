// ==================================================
// Copyright 2018(C) , DotLogix
// File:  StreamRouteResultWriterAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  13.08.2018
// ==================================================

#region
#endregion

using DotLogix.Core.Rest.Services.ResultWriters;

namespace DotLogix.Core.Rest.Services.Attributes {
    public class StreamRouteResultWriterAttribute : RouteResultWriterAttribute {
        public StreamRouteResultWriterAttribute() : base(() => StreamResultWriter.Instance) { }
    }
}
