// ==================================================
// Copyright 2018(C) , DotLogix
// File:  KeyValuePairNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Nodes.Converters;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An implementation of the <see cref="INodeConverterFactory"/> for key value pairs
    /// </summary>
    public class KeyValuePairNodeConverterFactory : NodeConverterFactoryBase {
        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter) {
            converter = null;

            if(typeSettings.NodeType != NodeTypes.Map)
                return false;

            var type = typeSettings.DataType.Type;
            if((type.IsGenericType == false) || (type.GetGenericTypeDefinition() != typeof(KeyValuePair<,>)))
                return false;

            const BindingFlags privateBindingFlags = BindingFlags.Instance|BindingFlags.NonPublic;
            if(resolver.TryResolve(typeSettings, typeSettings.DynamicType.GetField("key", privateBindingFlags), out var keySettings) == false)
                return false;
            if(resolver.TryResolve(typeSettings, typeSettings.DynamicType.GetField("value", privateBindingFlags), out var valueSettings) == false)
                return false;

            converter = new KeyValuePairNodeConverter(typeSettings, keySettings, valueSettings);
            return true;
        }
    }
}
