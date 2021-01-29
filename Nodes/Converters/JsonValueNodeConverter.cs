using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Formats.Json;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    ///     An implementation of the <see cref="INodeConverter" /> interface to convert primitives
    /// </summary>
    public class JsonValueNodeConverter : NodeConverter {
        /// <summary>
        ///     Creates a new instance of <see cref="ValueNodeConverter" />
        /// </summary>
        public JsonValueNodeConverter(TypeSettings typeSettings) : base(typeSettings) { }

        /// <inheritdoc />
        public override void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if(scopedSettings.ShouldEmitValue(instance) == false)
                return;

            writer.WriteValue(JsonPrimitives.FromObject(instance, settings, DataType));
        }

        public override object Read(INodeReader reader, IReadOnlyConverterSettings settings) {
            var value = reader.ReadValue();

            switch(value) {
                case IJsonPrimitive primitive:
                    try {
                        return primitive.ToObject(DataType, settings);
                    } catch(Exception) {
                        return default;
                    }
                default:
                    return value.TryConvertTo(Type, out value) ? value : default;
            }
        }

        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if(node.Type == NodeTypes.Empty)
                return Type.GetDefaultValue();

            if(node is NodeValue nodeValue && nodeValue.Value is IJsonPrimitive primitive)
                return primitive.ToObject(DataType, settings);
            throw new ArgumentException("Node is not a NodeValue");
        }
    }
}
