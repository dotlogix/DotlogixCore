// ==================================================
// Copyright 2016(C) , DotLogix
// File:  INodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  25.10.2017
// LastEdited:  25.10.2017
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Nodes.Io {
    public interface INodeReader {
        IEnumerable<NodeIoOp> EnumerateOps();
    }
}
