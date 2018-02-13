// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EndsWithWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public class EndsWithWebServiceRoute : WebServiceRouteBase {
        public EndsWithWebServiceRoute(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) : base(routeIndex, pattern, acceptedRequests, requestProcessor, priority) { }

        public override RouteMatch Match(HttpMethods method, string path) {
            if(((method & AcceptedRequests) != 0) && path.EndsWith(Pattern))
                return new RouteMatch(true, Pattern, Pattern.Length, null);
            return RouteMatch.Empty;
        }
    }
}
