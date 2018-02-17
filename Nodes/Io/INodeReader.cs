// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeReader.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Nodes.Io {
    public interface INodeReader {
        void CopyTo(INodeWriter nodeWriter);
    }
}
