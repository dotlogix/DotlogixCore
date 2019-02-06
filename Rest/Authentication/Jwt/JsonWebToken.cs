// ==================================================
// Copyright 2019(C) , DotLogix
// File:  JsonWebToken.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.01.2019
// LastEdited:  03.01.2019
// ==================================================

#region
using DotLogix.Core.Nodes;
#endregion

namespace DotLogix.Core.Rest.Authentication.Jwt {
    public class JsonWebToken<TPayload> {
        public NodeMap Header { get; }
        public TPayload Payload { get; }

        public JsonWebToken(NodeMap header, TPayload payload) {
            Header = header;
            Payload = payload;
        }
    }
}
