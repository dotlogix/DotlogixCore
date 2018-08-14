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
    public class Projection : IProjection {
        public GetterDelegate LeftGetter { get; }
        public GetterDelegate RightGetter { get; }
        public SetterDelegate LeftSetter { get; }
        public SetterDelegate RightSetter { get; }

        public Projection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter, SetterDelegate rightSetter) {
            LeftGetter = leftGetter;
            RightGetter = rightGetter;
            LeftSetter = leftSetter;
            RightSetter = rightSetter;
        }

        public virtual void ProjectLeftToRight(object left, object right) {
            RightSetter.Invoke(right, LeftGetter.Invoke(left));
        }

        public virtual void ProjectRightToLeft(object left, object right) {
            LeftSetter.Invoke(left, RightGetter.Invoke(right));
        }
    }
}
