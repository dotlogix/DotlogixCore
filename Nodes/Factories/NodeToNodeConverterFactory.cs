using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An implementation of the <see cref="INodeConverterFactory"/> for node values
    /// </summary>
    public class NodeToNodeConverterFactory : NodeConverterFactoryBase {
        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter) {
            var type = typeSettings.DataType.Type;
            if(type.IsAssignableTo(typeof(Node))) {
                converter = new NodeToNodeConverter(typeSettings);
                return true;
            }

            if(type == typeof(object))
            {
                converter = new NodeToNodeConverter(typeSettings, true);
                return true;
            }


            converter = null;
            return false;
        }
    }
}