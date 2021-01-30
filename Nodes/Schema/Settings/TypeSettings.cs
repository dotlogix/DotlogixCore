using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes.Schema {
    public class TypeSettings {
        public NodeTypes NodeType { get; set; }
        public DataType DataType { get; set; }
        public DynamicType DynamicType { get; set; }
        public INodeConverter Converter { get; set; }
        public TypeSettings ChildSettings { get; set; }
        public IReadOnlyConverterSettings Overrides { get; set; }

        public virtual void ApplyTo(TypeSettings settings) {
            settings.NodeType = NodeType;
            settings.DataType = DataType;
            settings.DynamicType = DynamicType;
            settings.Converter = Converter;
            settings.ChildSettings = ChildSettings;
            settings.Overrides = Overrides;
        }
    }
}