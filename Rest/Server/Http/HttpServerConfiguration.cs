// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Configuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Http.Parameters;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class HttpServerConfiguration : Settings{
        public static HttpServerConfiguration Default => new HttpServerConfiguration();

        public int MaxConcurrentRequests { get; set; } = 64;
        public int MaxConcurrentWebSockets { get; set; } = 64;
        public IParameterParser ParameterParser { get; set; } = PrimitiveParameterParser.Default;
    }
}
