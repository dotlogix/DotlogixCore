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
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Routing;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes {
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
    }
}
