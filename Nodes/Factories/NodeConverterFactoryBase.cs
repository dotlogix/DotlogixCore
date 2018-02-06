// ==================================================
// Copyright 2016(C) , DotLogix
// File:  NodeConverterFactoryBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.08.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    public abstract class NodeConverterFactoryBase : INodeConverterFactory {
        public INodeConverter CreateConverter(NodeTypes nodeType, DataType dataType) {
            if (TryCreateConverter(nodeType, dataType, out var converter))
                return converter;

            throw new InvalidOperationException($"Can not create converter for type {dataType.Type.GetFriendlyName()}");
        }

        public abstract bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out INodeConverter converter);
    }
}
