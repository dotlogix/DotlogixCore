// ==================================================
// Copyright 2018(C) , DotLogix
// File:  StartsWithWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public class StartsWithWebServiceRoute : WebServiceRouteBase {
        public StartsWithWebServiceRoute(string pattern, HttpMethods acceptedRequests,
                                         IWebRequestProcessor requestProcessor, int priority) :
            base(pattern, acceptedRequests, requestProcessor, priority) { }

        public override RouteMatch Match(HttpMethods method, string path) {
            if(((method & AcceptedRequests) != 0) && path.StartsWith(Pattern))
                return new RouteMatch(true, path, Pattern.Length, null);
            return RouteMatch.Empty;
        }
    }
}
