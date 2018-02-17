// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ObjectNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    public class ObjectNodeConverterFactory : NodeConverterFactoryBase {
        public override bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out INodeConverter converter) {
            converter = null;
            if(nodeType != NodeTypes.Map)
                return false;
            if((dataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Complex)
                return false;

            converter = new ObjectNodeConverter(dataType, AccessorTypes.Property, true);
            return true;
        }
    }
}
