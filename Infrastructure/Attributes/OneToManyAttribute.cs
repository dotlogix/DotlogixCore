using System;

namespace DotLogix.Architecture.Infrastructure.Attributes {
    public class OneToManyAttribute : NavigationAttribute {
        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute"></see> class.</summary>
        public OneToManyAttribute(Type targetType, string sourceProp = null, string targetProp = null) : base(targetType, RelationshipType.OneToMany, sourceProp, targetProp) { }
    }
}