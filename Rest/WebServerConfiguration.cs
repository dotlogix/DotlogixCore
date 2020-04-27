// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Configuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Http.Parameters;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Rest {
    public class WebServerConfiguration : Settings{
        public static WebServerConfiguration Default => new WebServerConfiguration();

        public int MaxConcurrentRequests { get; set; } = 64;
        public int MaxConcurrentWebSockets { get; set; } = 64;
        public IParameterParser ParameterParser { get; set; } = PrimitiveParameterParser.Default;
    }
}
