// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValueNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An implementation of the <see cref="INodeConverterFactory"/> for primitives
    /// </summary>
    public class ValueNodeConverterFactory : NodeConverterFactoryBase {
        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter) {
            converter = null;
            if(typeSettings.NodeType != NodeTypes.Value)
                return false;
            if((typeSettings.DataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Primitive)
                return false;
            converter = new ValueNodeConverter(typeSettings);
            return true;
        }
    }
}
