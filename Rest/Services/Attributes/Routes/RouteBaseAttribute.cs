// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteBaseAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    public abstract class RouteBaseAttribute : RouteAttribute {
        public RouteType RouteType { get; }

        protected RouteBaseAttribute(string pattern, RouteType routeType, int priority = 0) : base(pattern, priority) {
            RouteType = routeType;
        }

        protected override IWebServiceRoute CreateRoute(int routeIndex, string pattern, HttpMethods acceptedMethods, IWebRequestProcessor requestProcessor) {
            switch(RouteType) {
                case RouteType.Equals:
                    return new EqualsWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.StartsWith:
                    return new StartsWithWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.EndsWith:
                    return new EndsWithWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.Contains:
                    return new ContainsWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.Regex:
                    return new RegexWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.Pattern:
                    return new PatternWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.Fallback:
                    return new FallbackWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                default:
                    throw new ArgumentOutOfRangeException(nameof(RouteType), RouteType, null);
            }
        }
    }
}
