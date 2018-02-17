// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceRouteComparer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Rest.Server.Routes;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceRouteComparer : IComparer<IWebServiceRoute> {
        public static IComparer<IWebServiceRoute> Instance { get; } = new WebServiceRouteComparer();
        private WebServiceRouteComparer() { }

        public int Compare(IWebServiceRoute x, IWebServiceRoute y) {
            var priority = y.Priority.CompareTo(x.Priority);

            return priority == 0 ? y.RouteIndex.CompareTo(x.RouteIndex) : priority;
        }
    }
}
