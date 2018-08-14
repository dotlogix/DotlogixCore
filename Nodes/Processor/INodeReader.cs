// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public interface INodeReader {
        void CopyTo(INodeWriter writer);
        IEnumerable<NodeOperation> Read();
    }
}
