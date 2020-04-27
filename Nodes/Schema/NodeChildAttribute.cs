using System;

namespace DotLogix.Core.Nodes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class NodeChildAttribute : NodeTypeAttribute {
        /// <inheritdoc />
        public override void ApplyTo(INodeConverterResolver resolver, TypeSettings settings) {
            if(settings.ChildSettings == null)
                settings.ChildSettings = new TypeSettings();
            base.ApplyTo(resolver, settings.ChildSettings);
        }
    }
}