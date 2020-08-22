using System;

namespace DotLogix.Core.Nodes.Schema {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class NodeChildAttribute : NodeTypeAttribute {
        /// <inheritdoc />
        public override void ApplyTo(INodeConverterResolver resolver, TypeSettings settings) {
            settings.ChildSettings ??= new TypeSettings();
            base.ApplyTo(resolver, settings.ChildSettings);
        }
    }
}