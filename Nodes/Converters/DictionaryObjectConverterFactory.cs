using System;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Converters {
    public class DictionaryObjectConverterFactory : NodeConverterFactoryBase {
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out INodeConverter converter) {
            converter = null;
            if (typeSettings.NodeType != NodeTypes.List)
                return false;

            if ((typeSettings.DataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Collection)
                return false;

            var type = typeSettings.DataType.Type;
            if(type.IsGenericType == false)
                return false;

            if(type.IsAssignableToOpenGeneric(typeof(IDictionary<,>), out var genericParams) == false)
                return false;

            var keyType = genericParams[0].GetDataType();
            if ((keyType.Flags & DataTypeFlags.Primitive) != 0)
                return false;

            if (resolver.TryResolve(genericParams[1], out var valueSettings) == false)
                throw new InvalidOperationException($"Can not resolve a converter for value type of {type.GetFriendlyGenericName()}");

            if (type.IsInterface) {
                type = typeof(Dictionary<,>).MakeGenericType(genericParams);

                typeSettings.DynamicType = type.CreateDynamicType();
                typeSettings.DataType = type.ToDataType();
            }

            var converterType = typeof(DictionaryObjectConverter<,>).MakeGenericType(genericParams);
            converter = converterType.Instantiate<INodeConverter>(typeSettings, valueSettings);

            return true;
        }
    }
}