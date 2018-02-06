// ==================================================
// Copyright 2016(C) , DotLogix
// File:  INodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  30.08.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    public interface INodeConverterFactory {
        INodeConverter CreateConverter(NodeTypes nodeType, DataType dataType);
        bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out INodeConverter converter);
    }
}
