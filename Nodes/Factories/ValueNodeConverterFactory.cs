// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ValueNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.08.2017
// LastEdited:  09.09.2017
// ==================================================

#region
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    public class ValueNodeConverterFactory : NodeConverterFactoryBase {
        public override bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out INodeConverter converter) {
            converter = null;
            if(nodeType != NodeTypes.Value)
                return false;
            if((dataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Primitive)
                return false;
            converter = new ValueNodeConverter(dataType);
            return true;
        }
    }
}
