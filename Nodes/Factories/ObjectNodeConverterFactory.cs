// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ObjectNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.08.2017
// LastEdited:  06.09.2017
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
