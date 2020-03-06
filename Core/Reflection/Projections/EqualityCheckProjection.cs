// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EqualityCheckProjection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    /// <summary>
    /// A projection of two value accessors including an equality check
    /// </summary>
    public class EqualityCheckProjection : Projection {
        /// <summary>
        /// Creates a new instance of <see cref="EqualityCheckProjection"/>
        /// </summary>
        public EqualityCheckProjection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter, SetterDelegate rightSetter)
            : base(leftGetter, rightGetter, leftSetter, rightSetter) { }

        /// <inheritdoc />
        public override void ProjectLeftToRight(object left, object right) {
            if(ShouldProject(left, right, out var newValue, out _))
                RightSetter.Invoke(right, newValue);
        }

        /// <inheritdoc />
        public override void ProjectRightToLeft(object left, object right) {
            if(ShouldProject(left, right, out _, out var rightValue))
                LeftSetter.Invoke(left, rightValue);
        }

        private bool ShouldProject(object left, object right, out object leftValue, out object rightValue) {
            leftValue = LeftGetter.Invoke(left);
            rightValue = RightGetter.Invoke(right);

            return Equals(leftValue, rightValue) == false;
        }
    }
}
