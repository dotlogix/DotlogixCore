using System;
using DotLogix.Core.Nodes.Factories;
using DotLogix.Core.Utils.Naming;

namespace DotLogix.Core.Nodes {

    [AttributeUsage(AttributeTargets.Class)]
    public class NodeTypeAttribute : Attribute{
        public Type ConverterFactory { get; set; }
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
        public virtual void ApplyTo(INodeConverterResolver resolver, TypeSettings settings) {
            if(TimeFormat != null)
                settings.TimeFormat = TimeFormat;

            if(NumberFormat != null)
                settings.NumberFormat = NumberFormat;

            if(GuidFormat != null)
                settings.GuidFormat = GuidFormat;

            if(EnumFormat != null)
                settings.EnumFormat = EnumFormat;

            if(EmitMode != EmitMode.Inherit)
                settings.EmitMode = EmitMode;

            if(NamingStrategy != null && resolver.TryGet(NamingStrategy, out INamingStrategy strategy))
                settings.NamingStrategy = strategy;

            if(ConverterFactory != null && resolver.TryGet(ConverterFactory, out INodeConverterFactory factory))
                settings.Converter = factory.CreateConverter(resolver, settings);
        }
    }
}