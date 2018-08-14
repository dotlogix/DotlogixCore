// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeContainerType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    [Flags]
    public enum NodeContainerType {
        None,
        Map,
        List
    }
}
