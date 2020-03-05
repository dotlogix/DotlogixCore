using System.Threading.Tasks;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to node values
    /// </summary>
    public class NodeToNodeConverter : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="OptionalNodeConverter{TValue}"/>
        /// </summary>
        public NodeToNodeConverter(TypeSettings typeSettings, bool dynamic = false) : base(typeSettings) { }

        /// <inheritdoc />
        public override ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IConverterSettings settings) {
            var reader = new NodeReader((Node)instance);
            return reader.CopyToAsync(writer);
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IConverterSettings settings) {
            return node;

        }
    }
}