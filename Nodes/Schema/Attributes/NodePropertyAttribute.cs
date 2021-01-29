using System;

namespace DotLogix.Core.Nodes.Schema {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NodePropertyAttribute : NodeTypeAttribute {
        public string Name { get; set; }
        public int? Order { get; set; }

        /// <inheritdoc />
        public override void ApplyTo(INodeConverterResolver resolver, TypeSettings settings) {
            if(settings is MemberSettings memberSettings) {
                memberSettings.Name = Name;
                memberSettings.Order = Order;
            }
            base.ApplyTo(resolver, settings);
        }
    }
}