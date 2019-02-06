using System;
using System.Collections.Generic;
using System.Text;

namespace DotLogix.Architecture.Infrastructure.Attributes
{
    public class ManyToOneAttribute : NavigationAttribute {
        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute"></see> class.</summary>
        public ManyToOneAttribute(Type targetType, string sourceProp = null, string targetProp = null) : base(targetType, RelationshipType.ManyToOne, sourceProp, targetProp) { }
    }
}
