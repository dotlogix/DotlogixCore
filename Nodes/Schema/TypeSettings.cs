using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Nodes {
    public class TypeSettings : ConverterSettings{
        public DynamicType DynamicType { get; set; }
        public DataType DataType { get; set; }
        public NodeTypes NodeType { get; set; }

        /// <inheritdoc />
        public TypeSettings() { }

        /// <inheritdoc />
        public TypeSettings(ISettings settings) : base(settings) { }

        public IAsyncNodeConverter Converter { get; set; }

        public TypeSettings ChildSettings {
            get => GetWithMemberName<TypeSettings>();
            set => SetWithMemberName(value);
        }


        public virtual void Apply(TypeSettings settings) {
            Settings.Apply(settings.Settings);

            DataType = settings.DataType;
            NodeType = settings.NodeType;
            DynamicType = settings.DynamicType;

            NamingStrategy = settings.NamingStrategy;
            Converter = settings.Converter;
        }

        public bool ShouldEmitValue(object value, ConverterSettings settings) {
            var emitMode = EmitMode;
            emitMode = emitMode == EmitMode.Inherit ? settings.EmitMode : emitMode;

            if(value == null)
                return emitMode == EmitMode.Emit;

            var type = value.GetType();
            return (type.IsValueType == false) || (emitMode != EmitMode.IgnoreDefault) || (Equals(type.GetDefaultValue(), value) == false);
        }
    }
}