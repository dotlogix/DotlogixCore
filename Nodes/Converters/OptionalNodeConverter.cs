using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Converters
{
    public class OptionalNodeConverter<TValue> : NodeConverter
    {
        public OptionalNodeConverter(DataType dataType) : base(dataType) { }

        public override void Write(object instance, string rootName, INodeWriter writer) {
            var opt = (Optional<TValue>)instance;
            if(opt.IsDefined)
                Nodes.WriteTo(rootName, opt.Value, typeof(TValue), writer);
        }

        public override object ConvertToObject(Node node) {
            if(node.Type == NodeTypes.Empty)
                return new Optional<TValue>(default);

            var value = Nodes.ToObject<TValue>(node);
            return new Optional<TValue>(value);
        }
    }
}
