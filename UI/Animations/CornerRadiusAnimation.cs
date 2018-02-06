// ==================================================
// Copyright 2016(C) , DotLogix
// File:  CornerRadiusAnimation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
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
