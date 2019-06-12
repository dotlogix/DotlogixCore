using System;
using System.Collections.Generic;
using System.Text;

namespace DotLogix.Architecture.Infrastructure.Attributes
{
    /// <summary>
    /// An attribute to declare a many to one relationship
    /// </summary>
    public class ManyToOneAttribute : NavigationAttribute {
        /// <summary>
        /// Creates a new instance of <see cref="ManyToOneAttribute"/>
        /// </summary>
        public ManyToOneAttribute(Type targetType, string sourceProp = null, string targetProp = null) : base(targetType, RelationshipType.ManyToOne, sourceProp, targetProp) { }
    }
}
