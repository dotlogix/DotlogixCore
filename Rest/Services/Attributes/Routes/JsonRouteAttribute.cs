// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonRouteAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Processors.Json;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    public class JsonRouteAttribute : RouteBaseAttribute {
        public JsonRouteAttribute(string pattern = "", RouteType routeType = RouteType.StartsWith, int priority = 0) : base(pattern, routeType, priority) { }

        protected override IWebRequestProcessor CreateProcessor(IWebService serviceInstance, DynamicInvoke dynamicInvoke) {
            return new JsonWebRequestProcessor(serviceInstance, dynamicInvoke);
        }

        protected override void RegisterInitialProcessors(IWebServiceRoute route) {
            route.PreProcessors.Add(ParseJsonBodyPreProcessor.Instance);
            base.RegisterInitialProcessors(route);
        }
    }
}
