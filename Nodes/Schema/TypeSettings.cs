using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes {
    public class TypeSettings : ConverterSettings{
        public DynamicType DynamicType { get; set; }
        public DataType DataType { get; set; }
        public NodeTypes NodeType { get; set; }
        public IAsyncNodeConverter Converter { get; set; }
        public new TypeSettings ChildSettings { get; set; }
        public bool HasOverrides { get; set; }

        public virtual void ApplyTo(TypeSettings settings) {
            settings.DataType = DataType;
            settings.NodeType = NodeType;
            settings.DynamicType = DynamicType;
            settings.HasOverrides = HasOverrides;
            settings.Converter = Converter;
        }
    }
}