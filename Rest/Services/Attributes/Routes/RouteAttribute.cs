// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
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
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class RouteAttribute : Attribute {
        public string Pattern { get; }
        public int Priority { get; set; }

        protected RouteAttribute(string pattern, int priority=0) {
            Priority = priority;
            Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        }

        public IWebServiceRoute BuildRoute(IWebService webService, int routeIndex, DynamicInvoke dynamicInvoke, string pattern) {
            var requestProcessor = CreateProcessor(webService, dynamicInvoke);
            var acceptedMethods = GetAcceptedHttpMethods(dynamicInvoke);

            var route = CreateRoute(routeIndex, pattern, acceptedMethods, requestProcessor);
            RegisterInitialProcessors(route);
            return route;
        }

        protected virtual HttpMethods GetAcceptedHttpMethods(DynamicInvoke dynamicInvoke) {
            return dynamicInvoke.MethodInfo.GetCustomAttributes<HttpMethodAttribute>().Aggregate(HttpMethods.None, (m, a) => m | a.Methods);
        }

        protected abstract IWebRequestProcessor CreateProcessor(IWebService serviceInstance, DynamicInvoke dynamicInvoke);
        protected abstract IWebServiceRoute CreateRoute(int routeIndex, string pattern, HttpMethods acceptedMethods, IWebRequestProcessor requestProcessor);
        protected virtual void RegisterInitialProcessors(IWebServiceRoute route) { }
    }
}
