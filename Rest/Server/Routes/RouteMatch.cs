// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteMatch.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http.Parameters;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public class RouteMatch {
        public static readonly RouteMatch Empty = new RouteMatch(false, null, -1, null);
        public bool Success { get; }
        public string Match { get; }
        public int Length { get; }
        public IDictionary<string, Node> UrlParameters { get; }

        public RouteMatch(bool success, string match, int length, IDictionary<string, Node> parameters) {
            Success = success;
            Match = match;
            Length = length;
            UrlParameters = parameters;
        }
    }
}
