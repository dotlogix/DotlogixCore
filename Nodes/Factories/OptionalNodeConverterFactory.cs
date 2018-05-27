using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Factories {
    public class OptionalNodeConverterFactory : NodeConverterFactoryBase {

        public override bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out INodeConverter converter) {
            if(dataType.Type.IsAssignableToOpenGeneric(typeof(Optional<>), out var genericTypeArguments)) {
                var collectionConverterType = typeof(OptionalNodeConverter<>).MakeGenericType(genericTypeArguments);
                converter = (INodeConverter)Activator.CreateInstance(collectionConverterType, dataType);
                return true;
            }

            converter = null;
            return false;
        }
    }
}