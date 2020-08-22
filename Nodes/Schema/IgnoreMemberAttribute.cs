using System;

namespace DotLogix.Core.Nodes.Schema {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreMemberAttribute : Attribute { }
}