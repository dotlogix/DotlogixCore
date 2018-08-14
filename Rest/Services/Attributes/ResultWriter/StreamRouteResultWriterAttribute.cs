// ==================================================
// Copyright 2018(C) , DotLogix
// File:  StreamRouteResultWriterAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  13.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.ResultWriter {
    public class StreamRouteResultWriterAttribute : RouteResultWriterAttribute {
        protected StreamRouteResultWriterAttribute() : base(() => WebRequestResultStreamWriter.Instance) { }
    }
}
