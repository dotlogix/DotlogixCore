// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Nodes.Io;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    public interface INodeConverter {
        Type Type { get; }
        DataType DataType { get; }
        void Write(object instance, string rootName, INodeWriter writer);
        object ConvertToObject(Node node);
    }
}
