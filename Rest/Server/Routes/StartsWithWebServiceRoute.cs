// ==================================================
// Copyright 2018(C) , DotLogix
// File:  StartsWithWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public class StartsWithWebServiceRoute : WebServiceRouteBase {
        public StartsWithWebServiceRoute(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) : base(routeIndex, pattern, acceptedRequests, requestProcessor, priority) { }

        public override RouteMatch Match(HttpMethods method, string path) {
            if(((method & AcceptedRequests) != 0) && path.StartsWith(Pattern))
                return new RouteMatch(true, Pattern, Pattern.Length, null);
            return RouteMatch.Empty;
        }
    }
}
