// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TypeProjector.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    public class TypeProjector {
        protected List<IProjection> Projections;

        protected TypeProjector(Type left, Type right, IProjectionFactory factory = null) {
            if(left == null)
                throw new ArgumentNullException(nameof(left));
            if(right == null)
                throw new ArgumentNullException(nameof(right));

            if(factory == null)
                factory = ProjectionFactory.Auto;

            Projections = factory.CreateProjections(left, right).ToList();
        }

        protected TypeProjector(Type left, Type right, CreateProjectionsCallback callback) {
            if(left == null)
                throw new ArgumentNullException(nameof(left));
            if(right == null)
                throw new ArgumentNullException(nameof(right));
            if(callback == null)
                throw new ArgumentNullException(nameof(callback));
            Projections = callback.Invoke(left, right).ToList();
        }

        public virtual void ProjectLeftToRight(object left, object right) {
            foreach(var projection in Projections)
                projection.ProjectLeftToRight(left, right);
        }

        public virtual void ProjectRightToLeft(object left, object right) {
            foreach(var projection in Projections)
                projection.ProjectRightToLeft(left, right);
        }

        public static TypeProjector Create(Type left, Type right, IProjectionFactory factory = null) {
            return new TypeProjector(left, right, factory);
        }

        public static TypeProjector Create<TLeft, TRight>(IProjectionFactory factory = null) {
            return new TypeProjector(typeof(TLeft), typeof(TRight), factory);
        }

        public static TypeProjector Create(Type left, Type right, CreateProjectionsCallback callback) {
            return new TypeProjector(left, right, callback);
        }

        public static TypeProjector Create<TLeft, TRight>(CreateProjectionsCallback callback) {
            return new TypeProjector(typeof(TLeft), typeof(TRight), callback);
        }
    }
}
