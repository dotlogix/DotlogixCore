// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
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
