using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="INodeConverterFactory"/> for json primitives
    /// </summary>
    public class JsonValueNodeConverterFactory : NodeConverterFactoryBase {
        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out INodeConverter converter) {
            converter = null;
            if(typeSettings.NodeType != NodeTypes.Value)
                return false;
            if((typeSettings.DataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Primitive)
                return false;

            converter = new JsonValueNodeConverter(typeSettings);
            return true;
        }
    }
}