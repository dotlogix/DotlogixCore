using System;

namespace DotLogix.Architecture.Infrastructure.Attributes {
    /// <summary>
    /// An attribute to declare a one to one relationship
    /// </summary>
    public class OneToOneAttribute : NavigationAttribute {
        /// <summary>
        /// Creates a new instance of <see cref="OneToManyAttribute"/>
        /// </summary>
        public OneToOneAttribute(Type targetType, string sourceProp = null, string targetProp = null) : base(targetType, RelationshipType.OneToOne, sourceProp, targetProp) { }
    }
}