// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Projection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    /// <summary>
    /// A projection of two value accessors
    /// </summary>
    public class Projection : IProjection {
        /// <summary>
        /// The getter from the left side
        /// </summary>
        public GetterDelegate LeftGetter { get; }
        /// <summary>
        /// The getter from the right side
        /// </summary>
        public GetterDelegate RightGetter { get; }
        /// <summary>
        /// The setter from the left side
        /// </summary>
        public SetterDelegate LeftSetter { get; }
        /// <summary>
        /// The setter from the right side
        /// </summary>
        public SetterDelegate RightSetter { get; }

        /// <summary>
        /// Creates a new instance of <see cref="Projection"/>
        /// </summary>
        public Projection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter, SetterDelegate rightSetter) {
            LeftGetter = leftGetter;
            RightGetter = rightGetter;
            LeftSetter = leftSetter;
            RightSetter = rightSetter;
        }

        /// <summary>
        /// Project the left object to the right
        /// </summary>
        public virtual void ProjectLeftToRight(object left, object right) {
            RightSetter.Invoke(right, LeftGetter.Invoke(left));
        }

        /// <summary>
        /// Project the right object to the left
        /// </summary>
        public virtual void ProjectRightToLeft(object left, object right) {
            LeftSetter.Invoke(left, RightGetter.Invoke(right));
        }
    }
}
