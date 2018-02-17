// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ProjectionFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Reflection.Delegates;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    public class ProjectionFactory : IProjectionFactory {
        public static IProjectionFactory Auto { get; } = new ProjectionFactory();
        public static IProjectionFactory AutoWithEqualityCheck { get; } = new ProjectionFactory();

        protected ProjectionFactory() { }

        public virtual IEnumerable<IProjection> CreateProjections(Type left, Type right) {
            var dynamicLeft = left.CreateDynamicType(MemberTypes.Property);
            var dynamicRight = right.CreateDynamicType(MemberTypes.Property);

            foreach(var leftProperty in dynamicLeft.Properties) {
                var rightProperty = dynamicRight.GetPropery(leftProperty.Name);
                if(rightProperty == null)
                    continue;

                var leftGetter = leftProperty.Getter.GetterDelegate;
                var rightGetter = rightProperty.Getter.GetterDelegate;
                var leftSetter = leftProperty.Setter.SetterDelegate;
                var rightSetter = rightProperty.Setter.SetterDelegate;

                yield return CreateProjection(leftGetter, rightGetter, leftSetter, rightSetter);
            }
        }

        protected virtual IProjection CreateProjection(GetterDelegate leftGetter, GetterDelegate rightGetter,
                                                       SetterDelegate leftSetter, SetterDelegate rightSetter) {
            return new Projection(leftGetter, rightGetter, leftSetter, rightSetter);
        }
    }
}
