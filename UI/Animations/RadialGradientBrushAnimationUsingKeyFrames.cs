// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RadialGradientBrushAnimationUsingKeyFrames.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public class RadialGradientBrushAnimationUsingKeyFrames : AnimationUsingKeyFrames<RadialGradientBrush,
        RadialGradientBrushAnimationHelper, RadialGradientBrushKeyFrameCollection> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this RadialGradientBrushAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new RadialGradientBrushAnimationUsingKeyFrames Clone() {
            return (RadialGradientBrushAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new RadialGradientBrushAnimationUsingKeyFrames();
        }
        #endregion
    }
}
