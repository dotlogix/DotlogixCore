using System;

namespace DotLogix.Architecture.Infrastructure.Attributes {
    /// <summary>
    /// An attribute to declare a navigation attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NavigationAttribute : Attribute {
        /// <summary>
        /// The target type
        /// </summary>
        public Type TargetType { get; }
        /// <summary>
        /// The source property name
        /// </summary>
        public string SourceProp { get; }
        /// <summary>
        /// The target property name
        /// </summary>
        public string TargetProp { get; }
        /// <summary>
        /// The type of relationship between the properties
        /// </summary>
        public RelationshipType Type { get; }

        /// <summary>
        /// Creates a new instance of <see cref="NavigationAttribute"/>
        /// </summary>
        public NavigationAttribute(Type targetType, RelationshipType type, string sourceProp = null, string targetProp = null) {
            TargetType = targetType;
            Type = type;
            SourceProp = sourceProp;
            TargetProp = targetProp;
        }
    }
}