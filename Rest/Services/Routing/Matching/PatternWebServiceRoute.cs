// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PatternWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  31.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Utils.Patterns;
#endregion

namespace DotLogix.Core.Rest.Services.Routing {
    public class PatternWebServiceRoute : RegexWebServiceRoute {
        public static PatternParser PatternParser { get; } = PatternParser.Default.Clone();

        public PatternWebServiceRoute(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority)
        : base(routeIndex, PatternParser.ToRegexPattern(pattern), acceptedRequests, requestProcessor, priority) { }
    }
}
