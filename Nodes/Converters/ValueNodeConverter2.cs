using System;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert primitives
    /// </summary>
    public class ValueNodeConverter2 : NodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="ValueNodeConverter"/>
        /// </summary>
        public ValueNodeConverter2(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if (scopedSettings.ShouldEmitValue(instance) == false)
                return default;

            return writer.WriteValueAsync(name, instance);
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if (node.Type == NodeTypes.Empty)
                return Type.GetDefaultValue();

            if (node is NodeValue nodeValue)
                return nodeValue.GetValue(Type);
            throw new ArgumentException($"Expected node of type \"NodeValue\" got \"{node.Type}\"");
        }
    }
}