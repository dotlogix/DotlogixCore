// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteMatch.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Rest.Services.Routing {
    public class RouteMatch {
        public static readonly RouteMatch Empty = new RouteMatch(false, null, -1, null);
        public bool Success { get; }
        public string Match { get; }
        public int Length { get; }
        public IDictionary<string, object> UrlParameters { get; }

        public RouteMatch(bool success, string match, int length, IDictionary<string, object> parameters) {
            Success = success;
            Match = match;
            Length = length;
            UrlParameters = parameters;
        }
    }
}
