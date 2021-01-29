using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Utils.Naming;

namespace DotLogix.Core.Nodes.Schema {

    [AttributeUsage(AttributeTargets.Class)]
    public class NodeTypeAttribute : Attribute {
        /// <summary>
        ///     The custom converter factory type
        /// </summary>
        public Type ConverterFactory { get; set; }
        /// <summary>
        ///     The custom naming strategy type
        /// </summary>
        public Type NamingStrategy { get; set; }

        /// <summary>
        ///     The time format (u by default)
        /// </summary>
        public string TimeFormat { get; set; }

        /// <summary>
        ///     The number format (G by default)
        /// </summary>
        public string NumberFormat { get; set; }

        /// <summary>
        ///     The guid format (D by default)
        /// </summary>
        public string GuidFormat { get; set; }

        /// <summary>
        ///     The enum format (D by default)
        /// </summary>
        public string EnumFormat { get; set; }

        /// <summary>
        ///     Determines if default or null values should be ignored
        /// </summary>
        public EmitMode EmitMode { get; set; }

        /// <inheritdoc />
        public virtual void ApplyTo(INodeConverterResolver resolver, TypeSettings typeSettings) {
            var hasOverrides = false;
            var settings = new ConverterSettings(resolver);
            if (TimeFormat != null) {
                hasOverrides = true;
                settings.TimeFormat = TimeFormat;
            }

            if (NumberFormat != null) {
                hasOverrides = true;
                settings.NumberFormat = NumberFormat;
            }

            if (GuidFormat != null) {
                hasOverrides = true;
                settings.GuidFormat = GuidFormat;
            }

            if (EnumFormat != null) {
                hasOverrides = true;
                settings.EnumFormat = EnumFormat;
            }

            if (EmitMode != EmitMode.Inherit) {
                hasOverrides = true;
                settings.EmitMode = EmitMode;
            }

            if (NamingStrategy != null && resolver.TryGet(NamingStrategy, out INamingStrategy strategy)) {
                hasOverrides = true;
                settings.NamingStrategy = strategy;
            }

            if (ConverterFactory != null && resolver.TryGet(ConverterFactory, out INodeConverterFactory factory)) {
                typeSettings.Converter = factory.CreateConverter(resolver, typeSettings);
            }

            if (hasOverrides)
                typeSettings.Overrides = settings;
        }
    }
}