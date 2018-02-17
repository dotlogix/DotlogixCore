// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EqualityCheckProjectionFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Projections {
    public class EqualityCheckProjectionFactory : ProjectionFactory {
        protected override IProjection CreateProjection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter,
                                                        SetterDelegate rightSetter) {
            return new EqualityCheckProjection(leftGetter, rightGetter, leftSetter, rightSetter);
        }
    }
}
