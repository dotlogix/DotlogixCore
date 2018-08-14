// ==================================================
// Copyright 2018(C) , DotLogix
// File:  HttpMethods.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    [Flags]
    public enum HttpMethods {
        None = 0,
        Options = 1,
        Get = 2,
        Head = 4,
        Post = 8,
        Put = 16,
        Delete = 32,
        Trace = 64,
        Connect = 128,
        Any = Options | Get | Head | Post | Put | Delete | Trace | Connect
    }
}
