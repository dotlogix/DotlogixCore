// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EqualsWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region

using DotLogix.Core.Rest.Http;

#endregion

namespace DotLogix.Core.Rest.Services.Routing.Matching {
    public class EqualsMatchingStrategy : IRouteMatchingStrategy {
        public RouteMatch Match(HttpMethods method, string path) {
            if(((method & AcceptedMethods) != 0) && path.Equals(Pattern))
                return new RouteMatch(true, path, Pattern.Length, null);
            return RouteMatch.Empty;
        }

        public EqualsMatchingStrategy(string pattern, HttpMethods acceptedRequests, bool isRooted = false) {
            Pattern = pattern;
            AcceptedMethods = acceptedRequests;
            IsRooted = isRooted;
        }

        public bool IsRooted { get; }
        public string Pattern { get; }
        public HttpMethods AcceptedMethods { get; }
    }
}
