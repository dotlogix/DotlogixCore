// ==================================================
// Copyright 2016(C) , DotLogix
// File:  NodeTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  18.08.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Nodes {
    [Flags]
    public enum NodeTypes {
        None = 0 << 0,
        Empty = 1 << 0,
        Value = 1 << 1,
        List = 1 << 2,
        Map = 1 << 3
    }
}
