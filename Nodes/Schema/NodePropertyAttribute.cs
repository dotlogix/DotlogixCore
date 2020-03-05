using System;

namespace DotLogix.Core.Nodes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NodePropertyAttribute : NodeTypeAttribute {
        public string Name { get; set; }
        public int? Order { get; set; }
    }
}