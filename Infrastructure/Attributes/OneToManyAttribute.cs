using System;

namespace DotLogix.Architecture.Infrastructure.Attributes {
    /// <summary>
    /// An attribute to declare a one to many relationship
    /// </summary>
    public class OneToManyAttribute : NavigationAttribute {
        /// <summary>
        /// Creates a new instance of <see cref="OneToManyAttribute"/>
        /// </summary>
        public OneToManyAttribute(Type targetType, string sourceProp = null, string targetProp = null) : base(targetType, RelationshipType.OneToMany, sourceProp, targetProp) { }
    }
}