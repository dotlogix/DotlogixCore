// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LinearGradientBrushAnimationUsingKeyFrames.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public class LinearGradientBrushAnimationUsingKeyFrames : AnimationUsingKeyFrames<LinearGradientBrush,
        LinearGradientBrushAnimationHelper, LinearGradientBrushKeyFrameCollection> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this LinearGradientBrushAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new LinearGradientBrushAnimationUsingKeyFrames Clone() {
            return (LinearGradientBrushAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new LinearGradientBrushAnimationUsingKeyFrames();
        }
        #endregion
    }
}
