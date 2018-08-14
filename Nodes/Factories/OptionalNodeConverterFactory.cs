// ==================================================
// Copyright 2018(C) , DotLogix
// File:  OptionalNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  20.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    public class OptionalNodeConverterFactory : NodeConverterFactoryBase {
        public override bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out INodeConverter converter) {
            if(dataType.Type.IsAssignableToOpenGeneric(typeof(Optional<>), out var genericTypeArguments)) {
                var collectionConverterType = typeof(OptionalNodeConverter<>).MakeGenericType(genericTypeArguments);
                converter = (INodeConverter)Activator.CreateInstance(collectionConverterType, dataType);
                return true;
            }

            converter = null;
            return false;
        }
    }
}
