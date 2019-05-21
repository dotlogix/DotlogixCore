// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Core.Nodes.Processor {
    public interface INodeReader : IDisposable {
        void CopyTo(INodeWriter writer);
        IEnumerable<NodeOperation> Read();
    }
}
