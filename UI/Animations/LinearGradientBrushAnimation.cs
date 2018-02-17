// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LinearGradientBrushAnimation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public class LinearGradientBrushAnimation : RefTypeAnimation<LinearGradientBrush, LinearGradientBrushAnimationHelper
    > {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this LinearGradientBrushAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new LinearGradientBrushAnimation Clone() {
            return (LinearGradientBrushAnimation)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new LinearGradientBrushAnimation();
        }
        #endregion
    }
}
