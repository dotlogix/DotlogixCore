// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SimpleRouteAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Processors.Base;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Routes {
    public class SimpleRouteAttribute : RouteBaseAttribute {
        public SimpleRouteAttribute(string pattern = "", RouteType routeType = RouteType.StartsWith, int priority=0) : base(pattern, routeType, priority) { }

        protected override IWebRequestProcessor CreateProcessor(IWebService serviceInstance, DynamicInvoke dynamicInvoke) {
            return new WebRequestProcessor(serviceInstance, dynamicInvoke);
        }
    }
}
