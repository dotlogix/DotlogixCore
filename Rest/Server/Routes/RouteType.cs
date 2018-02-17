// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Rest.Server.Routes {
    public enum RouteType {
        Equals,
        StartsWith,
        EndsWith,
        Contains,
        Regex,
        Pattern,
        Fallback
    }
}
