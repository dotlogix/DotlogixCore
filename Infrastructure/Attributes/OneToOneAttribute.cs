﻿using System;

namespace DotLogix.Architecture.Infrastructure.Attributes {
    public class OneToOneAttribute : NavigationAttribute {
        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute"></see> class.</summary>
        public OneToOneAttribute(Type targetType, string sourceProp = null, string targetProp = null) : base(targetType, RelationshipType.OneToOne, sourceProp, targetProp) { }
    }
}