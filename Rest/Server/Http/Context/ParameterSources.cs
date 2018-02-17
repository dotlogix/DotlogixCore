// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ParameterSources.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    [Flags]
    public enum ParameterSources {
        None = 0,
        Header = 1 << 0,
        Query = 1 << 1,
        Url = 1 << 2,
        UserDefined = 1 << 3,
        Any = Header | Query | Url | UserDefined
    }
}
