using System;

namespace DotLogix.Architecture.Infrastructure.Attributes {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NavigationAttribute : Attribute {
        public Type TargetType { get; }
        public string SourceProp { get; }
        public string TargetProp { get; }
        public RelationshipType Type { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute"></see> class.</summary>
        public NavigationAttribute(Type targetType, RelationshipType type, string sourceProp = null, string targetProp = null) {
            TargetType = targetType;
            Type = type;
            SourceProp = sourceProp;
            TargetProp = targetProp;
        }
    }
}