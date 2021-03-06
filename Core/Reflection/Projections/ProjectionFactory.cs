﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ProjectionFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using DotLogix.Core.Reflection.Delegates;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    /// <summary>
    /// A factory to produce projectsions
    /// </summary>
    public class ProjectionFactory : IProjectionFactory {
        /// <summary>
        /// Try to map as many properties as possible
        /// </summary>
        public static IProjectionFactory Auto { get; } = new ProjectionFactory();
        /// <summary>
        /// Try to map as many properties as possible and as little as necessary include an equality check before assignment
        /// </summary>
        public static IProjectionFactory AutoWithEqualityCheck { get; } = new EqualityCheckProjectionFactory();
        /// <summary>
        /// Try to map as many properties as possible and undefined <see cref="Optional{TValue}"/> are ignored include an equality check before assignment
        /// </summary>
        public static IProjectionFactory AutoOptional { get; } = new OptionalProjectionFactory();

        /// <summary>
        /// Create a new instance of <see cref="ProjectionFactory"/>
        /// </summary>
        protected ProjectionFactory() { }

        /// <inheritdoc />
        public virtual IEnumerable<IProjection> CreateProjections(Type left, Type right) {
            var dynamicLeft = left.CreateDynamicType();
            var dynamicRight = right.CreateDynamicType();

            foreach(var leftProperty in dynamicLeft.Properties) {
                var rightProperty = dynamicRight.GetProperty(leftProperty.Name);
                if(rightProperty == null)
                    continue;

                var leftGetter = leftProperty.Getter.GetterDelegate;
                var rightGetter = rightProperty.Getter.GetterDelegate;
                var leftSetter = leftProperty.Setter.SetterDelegate;
                var rightSetter = rightProperty.Setter.SetterDelegate;

                yield return CreateProjection(leftGetter, rightGetter, leftSetter, rightSetter);
            }
        }

        /// <summary>
        /// Create a new projection
        /// </summary>
        protected virtual IProjection CreateProjection(GetterDelegate leftGetter, GetterDelegate rightGetter,
                                                       SetterDelegate leftSetter, SetterDelegate rightSetter) {
            return new Projection(leftGetter, rightGetter, leftSetter, rightSetter);
        }
    }
}
