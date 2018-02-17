// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeIoState.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Io {
    [Flags]
    public enum NodeIoState {
        None,
        InsideMap,
        InsideList
    }
}
