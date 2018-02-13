// ==================================================
// Copyright 2018(C) , DotLogix
// File:  HttpMethods.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  13.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    [Flags]
    public enum HttpMethods {
        None = 0,
        Options = 1 << 0,
        Get = 1 << 1,
        Head = 1 << 2,
        Post = 1 << 3,
        Put = 1 << 4,
        Delete = 1 << 5,
        Trace = 1 << 6,
        Connect = 1 << 7
        Any = Options | Get | Head | Post | Put | Delete | Trace | Connect
    }
}
