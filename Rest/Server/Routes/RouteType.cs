// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RouteType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

namespace DotLogix.Core.Rest.Server.Routes {
    public enum RouteType {
        Equals,
        StartsWith,
        EndsWith,
        Contains,
        Regex,
        Pattern,
    }
}
