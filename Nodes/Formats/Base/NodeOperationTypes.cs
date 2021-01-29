// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeOperationTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

using System;

namespace DotLogix.Core.Nodes.Formats {
    [Flags]
    public enum NodeOperationTypes {
        None = 0,
        BeginMap = 1 << 0,
        EndMap = 1 << 1,
        BeginList = 1 << 2,
        EndList = 1 << 3,
        Value = 1 << 4,

        BeginAny = BeginMap | BeginList,
        EndAny = EndMap | EndList
    }
}
