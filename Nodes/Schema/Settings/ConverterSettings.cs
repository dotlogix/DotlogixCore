#region using
using System;
using System.Globalization;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Utils.Naming;
#endregion

namespace DotLogix.Core.Nodes.Schema {
    public class ConverterSettings : IConverterSettings {
        public static INodeConverterResolver DefaultResolver => CreateDefaultResolver();

        private static INodeConverterResolver CreateDefaultResolver() {
            var resolver = new NodeConverterResolver();
            resolver.Add(new ObjectNodeConverterFactory());
            resolver.Add(new OptionalNodeConverterFactory());
            resolver.Add(new KeyValuePairNodeConverterFactory());
            resolver.Add(new CollectionNodeConverterFactory());
            resolver.Add(new ValueNodeConverterFactory());
            return resolver;
        }
        
        public ConverterSettings(INodeConverterResolver resolver) {
            Resolver = resolver;
        }

        /// <summary>
        ///     The default settings
        /// </summary>
        public static ConverterSettings Default => new ConverterSettings(DefaultResolver);

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        ///     The naming strategy (camelCase by default)
        /// </summary>
        public INamingStrategy NamingStrategy { get; set; } = NamingStrategies.CamelCase;

        /// <summary>
        ///     The time format (o by default)
        /// </summary>
        public string TimeFormat { get; set; } = "o";

        /// <summary>
        ///     The number format (G by default)
        /// </summary>
        public string NumberFormat { get; set; } = "G";

        /// <summary>
        ///     The guid format (D by default)
        /// </summary>
        public string GuidFormat { get; set; } = "D";

        /// <summary>
        ///     The enum format (D by default)
        /// </summary>
        public string EnumFormat { get; set; } = "D";

        /// <summary>
        ///     Determines if default or null values should be ignored
        /// </summary>
        public EmitMode EmitMode { get; set; } = EmitMode.Emit;

        /// <summary>
        ///     The converter resolver
        /// </summary>
        public INodeConverterResolver Resolver { get; }

        public virtual IReadOnlyConverterSettings GetScoped(TypeSettings typeSettings = null, TypeSettings memberSettings = null) {
            var typeOverrides = typeSettings?.Overrides;
            var memberOverrides = memberSettings?.Overrides;
            
            if (typeOverrides != null || memberOverrides != null)
                return new ScopedConverterSettings(this, typeOverrides, memberOverrides);
            return this;
        }

        public virtual bool ShouldEmitValue(object value) {
            return ShouldEmitValue(value, EmitMode);
        }

        public static bool ShouldEmitValue(object value, EmitMode emitMode) {
            switch(emitMode) {
                case EmitMode.Inherit:
                case EmitMode.Emit:
                    return true;
                case EmitMode.IgnoreNull:
                    return value != null;
                case EmitMode.IgnoreDefault:
                    if(value == null)
                        return false;
                    
                    var type = value.GetType();
                    return !(type.IsValueType && Equals(type.GetDefaultValue(), value));
                default:
                    throw new ArgumentOutOfRangeException(nameof(emitMode), emitMode, null);
            }
        }
    }
}
