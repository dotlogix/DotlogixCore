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
        private readonly bool _useJsonValues;

        /// <inheritdoc />
        public ValueNodeConverterFactory(bool useJsonValues) {
            _useJsonValues = useJsonValues;
        }

        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter) {
            converter = null;
            if(typeSettings.NodeType != NodeTypes.Value)
                return false;
            if((typeSettings.DataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Primitive)
                return false;

            //if(_useJsonValues)
            //    converter = new JsonValueNodeConverter(typeSettings);
            //else
                converter = new ValueNodeConverter(typeSettings);
            return true;
        }
    }
}
