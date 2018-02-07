// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteBaseAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.01.2018
// LastEdited:  31.01.2018
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

        protected RouteBaseAttribute(string pattern, RouteType routeType) : base(pattern) {
            RouteType = routeType;
        }

        protected override IWebServiceRoute CreateRoute(string pattern, HttpMethods acceptedMethods, IWebRequestProcessor requestProcessor) {
            switch(RouteType) {
                case RouteType.StartsWith:
                    return new StartsWithWebServiceRoute(pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.EndsWith:
                    return new EndsWithWebServiceRoute(pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.Contains:
                    return new ContainsWebServiceRoute(pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.Regex:
                    return new RegexWebServiceRoute(pattern, acceptedMethods, requestProcessor, Priority);
                case RouteType.Pattern:
                    return new PatternWebServiceRoute(pattern, acceptedMethods, requestProcessor, Priority);
                default:
                    throw new ArgumentOutOfRangeException(nameof(RouteType), RouteType, null);
            }
        }
    }
}
