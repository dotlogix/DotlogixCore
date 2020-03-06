using System;

namespace DotLogix.Core.Nodes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class NodeChildAttribute : NodeTypeAttribute {
    }
}