// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Rest.Server.Routes {
    public enum RouteMatchingStrategy {
        Equals,
        StartsWith,
        EndsWith,
        Contains,
        Regex,
        Pattern,
        Fallback
    }
}
