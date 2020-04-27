using System;
using DotLogix.Core.Nodes.Factories;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NodePropertyAttribute : NodeTypeAttribute {
        public string Name { get; set; }
        public int? Order { get; set; }

        /// <inheritdoc />
        public override void ApplyTo(INodeConverterResolver resolver, TypeSettings settings) {
            if(settings is MemberSettings memberSettings) {
                if(memberSettings.Name != null)
                    memberSettings.Name = Name;

                if(memberSettings.Order.HasValue)
                    memberSettings.Order = Order;
            }

            bool hasOverrides = false;
            if(TimeFormat != null)
            {
                hasOverrides = true;
                settings.TimeFormat = TimeFormat;
            }

            if(NumberFormat != null)
            {
                hasOverrides = true;
                settings.NumberFormat = NumberFormat;
            }

            if (GuidFormat != null)
            {
                hasOverrides = true;
                settings.GuidFormat = GuidFormat;
            }

            if (EnumFormat != null)
            {
                hasOverrides = true;
                settings.EnumFormat = EnumFormat;
            }

            if (EmitMode != EmitMode.Inherit)
            {
                hasOverrides = true;
                settings.EmitMode = EmitMode;
            }

            if (NamingStrategy != null && resolver.TryGet(NamingStrategy, out INamingStrategy strategy))
            {
                hasOverrides = true;
                settings.NamingStrategy = strategy;
            }

            if (ConverterFactory != null && resolver.TryGet(ConverterFactory, out INodeConverterFactory factory))
            {
                hasOverrides = true;
                settings.Converter = factory.CreateConverter(resolver, settings);
            }

            settings.HasOverrides |= hasOverrides;
        }
    }
}