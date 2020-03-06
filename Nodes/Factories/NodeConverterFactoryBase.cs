// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeConverterFactoryBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An base class implementation of the <see cref="INodeConverterFactory"/>
    /// </summary>
    public abstract class NodeConverterFactoryBase : INodeConverterFactory {
        /// <summary>
        /// Create a new node converter
        /// </summary>
        public IAsyncNodeConverter CreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings) {
            if (TryCreateConverter(resolver, typeSettings, out var converter))
                return converter;

            throw new InvalidOperationException($"Can not create converter for type {typeSettings.DataType.Type.GetFriendlyName()}");
        }

        /// <summary>
        /// Try to create a new node converter
        /// </summary>
        public abstract bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter);
    }
}
