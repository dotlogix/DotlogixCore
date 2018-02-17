// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RefTypeAnimation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Generic animation class for TValue reference type. Uses TAnimationHelper to animate value. For using in XAML
    ///     subclass it with non-generic class.
    /// </summary>
    public class RefTypeAnimation<TValue, TAnimationHelper> :
    Animation<TValue, TValue, TAnimationHelper>
        where TValue : class
        where TAnimationHelper : class, IAnimationHelper<TValue>, new() {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new RefTypeAnimation<TValue, TAnimationHelper>();
        }
        #endregion
    }
}
