// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeIoOperation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Nodes.Io {
    public struct NodeIoOperation {
        public NodeIoState NextState;
        public NodeIoOpCodes OpCode;
        public NodeIoOpCodes AllowedNextOpCodes;
    }
}
