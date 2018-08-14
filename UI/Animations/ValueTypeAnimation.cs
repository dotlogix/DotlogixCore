// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ValueTypeAnimation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Generic animation class for TValue ValueType type. Uses TAnimationHelper to animate value. For using in XAML
    ///     subclass it with non-generic class.
    /// </summary>
    public class ValueTypeAnimation<TValue, TAnimationHelper> :
    Animation<TValue, TValue?, TAnimationHelper>
        where TValue : struct
        where TAnimationHelper : class, IAnimationHelper<TValue>, new() {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new ValueTypeAnimation<TValue, TAnimationHelper>();
        }
        #endregion
    }
}
