// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicRouteAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Processors.Dynamic;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    public class DynamicRouteAttribute : RouteBaseAttribute {
        public DynamicRouteAttribute(string pattern = "", RouteType routeType = RouteType.StartsWith, int priority = 0) : base(pattern, routeType, priority) { }

        protected override IWebRequestProcessor CreateProcessor(IWebService serviceInstance, DynamicInvoke dynamicInvoke) {
            return new DynamicWebRequestProcessor(serviceInstance, dynamicInvoke);
        }
    }
}
