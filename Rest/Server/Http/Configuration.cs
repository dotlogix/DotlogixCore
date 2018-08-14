// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Configuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Server.Http.Context;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class Configuration {
        public static Configuration Default => new Configuration();
        public int MaxConcurrentRequests { get; }
        public bool UseExtendedParsing { get; }
        public ExtendedParameterParser ParameterParser { get; }

        public Configuration(int maxConcurrentRequests = 64, bool useExtendedParsing = true) {
            MaxConcurrentRequests = maxConcurrentRequests;
            UseExtendedParsing = useExtendedParsing;
            ParameterParser = useExtendedParsing ? ExtendedParameterParser.Default : null;
        }
    }
}
