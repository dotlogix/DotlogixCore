// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CornerRadiusAnimation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    public class CornerRadiusAnimation : ValueTypeAnimation<CornerRadius, CornerRadiusAnimationHelper> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this CornerRadiusAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new CornerRadiusAnimation Clone() {
            return (CornerRadiusAnimation)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new CornerRadiusAnimation();
        }
        #endregion
    }
}
