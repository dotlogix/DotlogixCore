// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Attributes.Http;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Processors.Base;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteAttribute : Attribute {
        public string Pattern { get; set; }
        public RouteMatchingStrategy MatchingStrategy { get; set; }
        public int Priority { get; set; }
        public bool IsRooted { get; set; }

        public RouteAttribute(string pattern = "", RouteMatchingStrategy matchingStrategy = RouteMatchingStrategy.Equals, int priority = 0) {
            Priority = priority;
            Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
            MatchingStrategy = matchingStrategy;
        }

        public IWebServiceRoute BuildRoute(IWebService webService, int routeIndex, DynamicInvoke dynamicInvoke, string pattern) {
            var requestProcessor = CreateProcessor(webService, dynamicInvoke);
            var acceptedMethods = GetAcceptedHttpMethods(dynamicInvoke);

            var route = CreateRoute(routeIndex, pattern, acceptedMethods, requestProcessor);
            route.IsRooted |= IsRooted;
            RegisterInitialProcessors(route);
            return route;
        }

        protected virtual IWebServiceRoute CreateRoute(int routeIndex, string pattern, HttpMethods acceptedMethods, IWebRequestProcessor requestProcessor) {
            switch(MatchingStrategy) {
                case RouteMatchingStrategy.Equals:
                    return new EqualsWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteMatchingStrategy.StartsWith:
                    return new StartsWithWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteMatchingStrategy.EndsWith:
                    return new EndsWithWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteMatchingStrategy.Contains:
                    return new ContainsWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteMatchingStrategy.Regex:
                    return new RegexWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteMatchingStrategy.Pattern:
                    return new PatternWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                case RouteMatchingStrategy.Fallback:
                    return new FallbackWebServiceRoute(routeIndex, pattern, acceptedMethods, requestProcessor, Priority);
                default:
                    throw new ArgumentOutOfRangeException(nameof(RouteMatchingStrategy), MatchingStrategy, null);
            }
        }
        protected virtual IWebRequestProcessor CreateProcessor(IWebService serviceInstance, DynamicInvoke dynamicInvoke) {
            return new WebRequestProcessor(serviceInstance, dynamicInvoke, Priority, false);
        }

        protected virtual void RegisterInitialProcessors(IWebServiceRoute route) { }

        protected virtual HttpMethods GetAcceptedHttpMethods(DynamicInvoke dynamicInvoke) {
            return dynamicInvoke.MethodInfo.GetCustomAttributes<HttpMethodAttribute>().Aggregate(HttpMethods.None, (m, a) => m | a.Methods);
        }
    }
}
