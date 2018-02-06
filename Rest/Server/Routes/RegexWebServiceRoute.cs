// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RegexWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System.Linq;
using System.Text.RegularExpressions;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Parameters;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public class RegexWebServiceRoute : WebServiceRouteBase {
        public Regex Regex { get; }

        public RegexWebServiceRoute(string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor,
                                    int priority) : base(pattern, acceptedRequests, requestProcessor, priority) {
            Regex = new Regex(Pattern);
        }


        public override RouteMatch Match(HttpMethods method, string path) {
            if((AcceptedRequests & method) == 0)
                return RouteMatch.Empty;
            var names = Regex.GetGroupNames();
            var match = Regex.Match(path);
            if(!match.Success)
                return RouteMatch.Empty;
            var parameters = names.Select(n => new Parameter(n, match.Groups[n].Value)).ToList();
            return new RouteMatch(true, match.Value, match.Length, parameters);
        }
    }
}
