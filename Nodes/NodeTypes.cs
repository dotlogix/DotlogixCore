// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
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
