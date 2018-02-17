// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeReaderBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Nodes.Io {
    public abstract class NodeReaderBase : INodeReader {
        public abstract void CopyTo(INodeWriter nodeWriter);
    }
}
