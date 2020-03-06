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
#endregion

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An implementation of the <see cref="INodeConverterFactory"/> for optional values
    /// </summary>
    public class OptionalNodeConverterFactory : NodeConverterFactoryBase {
        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter) {
            if(typeSettings.DataType.Type.IsAssignableToOpenGeneric(typeof(Optional<>), out var genericTypeArguments)) {
                var collectionConverterType = typeof(OptionalNodeConverter<>).MakeGenericType(genericTypeArguments);
                converter = (IAsyncNodeConverter)Activator.CreateInstance(collectionConverterType, typeSettings);
                return true;
            }

            converter = null;
            return false;
        }
    }
}
