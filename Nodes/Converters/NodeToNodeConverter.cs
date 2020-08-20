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
        public override async ValueTask WriteAsync(object instance, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings) {
            var reader = new NodeReader((Node)instance);
            await reader.CopyToAsync(writer).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override ValueTask<object> ReadAsync(IAsyncNodeReader reader, IReadOnlyConverterSettings settings) {
            return default;
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            return node;

        }
    }
}