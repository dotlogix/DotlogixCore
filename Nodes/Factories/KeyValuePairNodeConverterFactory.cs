// ==================================================
// Copyright 2016(C) , DotLogix
// File:  KeyValuePairNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.08.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    public class KeyValuePairNodeConverterFactory : NodeConverterFactoryBase
    {
        public override bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out INodeConverter converter)
        {
            converter = null;

            if (nodeType != NodeTypes.Map)
                return false;

            var type = dataType.Type;
            if (type.IsGenericType == false || type.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
                return false;

            converter = new KeyValuePairNodeConverter(dataType);
            return true;
        }
    }
}
