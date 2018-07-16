// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RegexWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Parameters;
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

            var parameters = new Dictionary<string, Node>();
            foreach (var name in names)
            {
                var group = match.Groups[name];
                if (group.Captures.Count > 1) {
                    var nodeList = new NodeList();
                    foreach (Capture capture in group.Captures)
                        nodeList.CreateValue(capture.Value);
                    parameters.Add(name, nodeList);
                } else {
                    parameters.Add(name, new NodeValue(group.Value));
                }

            }

            return new RouteMatch(true, match.Value, match.Length, parameters);
        }
    }
}
