// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeOperationTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Nodes.Processor {
    public enum NodeOperationTypes {
        BeginMap = 1 << 0,
        EndMap = 1 << 1,
        BeginList = 1 << 2,
        EndList = 1 << 3,
        Value = 1 << 4,
        AutoComplete = 1 << 5,
        BeginAny = BeginMap | BeginList | Value
    }
}
