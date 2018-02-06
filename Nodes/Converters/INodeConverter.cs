// ==================================================
// Copyright 2016(C) , DotLogix
// File:  INodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  24.08.2017
// LastEdited:  06.09.2017
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
