// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TypeProjector.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    /// <summary>
    /// A type to type projector
    /// </summary>
    public class TypeProjector {
        /// <summary>
        /// The projections
        /// </summary>
        public List<IProjection> Projections { get; }

        /// <summary>
        /// Create a new instance of <see cref="TypeProjector"/>
        /// </summary>
        protected TypeProjector(Type left, Type right, IProjectionFactory factory = null) {
            if(left == null)
                throw new ArgumentNullException(nameof(left));
            if(right == null)
                throw new ArgumentNullException(nameof(right));

            if(factory == null)
                factory = ProjectionFactory.Auto;

            Projections = factory.CreateProjections(left, right).ToList();
        }

        /// <summary>
        /// Create a new instance of <see cref="TypeProjector"/>
        /// </summary>
        protected TypeProjector(Type left, Type right, CreateProjectionsCallback callback) {
            if(left == null)
                throw new ArgumentNullException(nameof(left));
            if(right == null)
                throw new ArgumentNullException(nameof(right));
            if(callback == null)
                throw new ArgumentNullException(nameof(callback));
            Projections = callback.Invoke(left, right).ToList();
        }

        /// <summary>
        /// Project the left object to the right
        /// </summary>
        public virtual void ProjectLeftToRight(object left, object right) {
            foreach(var projection in Projections)
                projection.ProjectLeftToRight(left, right);
        }

        /// <summary>
        /// Project the right object to the left
        /// </summary>
        public virtual void ProjectRightToLeft(object left, object right) {
            foreach(var projection in Projections)
                projection.ProjectRightToLeft(left, right);
        }

        /// <summary>
        /// Create a new type projector
        /// </summary>
        public static TypeProjector Create(Type left, Type right, IProjectionFactory factory = null) {
            return new TypeProjector(left, right, factory);
        }

        /// <summary>
        /// Create a new type projector
        /// </summary>
        public static TypeProjector Create<TLeft, TRight>(IProjectionFactory factory = null) {
            return new TypeProjector(typeof(TLeft), typeof(TRight), factory);
        }

        /// <summary>
        /// Create a new type projector
        /// </summary>
        public static TypeProjector Create(Type left, Type right, CreateProjectionsCallback callback) {
            return new TypeProjector(left, right, callback);
        }

        /// <summary>
        /// Create a new type projector
        /// </summary>
        public static TypeProjector Create<TLeft, TRight>(CreateProjectionsCallback callback) {
            return new TypeProjector(typeof(TLeft), typeof(TRight), callback);
        }
    }
}
