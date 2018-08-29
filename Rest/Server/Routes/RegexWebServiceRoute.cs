// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RegexWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public class RegexWebServiceRoute : WebServiceRouteBase {
        public Regex Regex { get; }

        public RegexWebServiceRoute(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) : base(routeIndex, pattern, acceptedRequests, requestProcessor, priority) {
            Regex = new Regex(Pattern);
        }


        public override RouteMatch Match(HttpMethods method, string path) {
            if((AcceptedRequests & method) == 0)
                return RouteMatch.Empty;
            var names = Regex.GetGroupNames();
            var match = Regex.Match(path);
            if(!match.Success)
                return RouteMatch.Empty;

            var parameters = new Dictionary<string, object>();
            foreach(var name in names) {
                var group = match.Groups[name];
                if(group.Captures.Count > 1) {
                    var values = new string[group.Captures.Count];
                    for(int i = 0; i < group.Captures.Count; i++) {
                        values[i] = group.Captures[i].Value;
                    }
                    parameters.Add(name, values);
                } else
                    parameters.Add(name, group.Value);
            }

            return new RouteMatch(true, match.Value, match.Length, parameters);
        }
    }
}
