// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CornerRadiusAnimationUsingKeyFrames.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    public class CornerRadiusAnimationUsingKeyFrames : AnimationUsingKeyFrames<CornerRadius, CornerRadiusAnimationHelper
        , CornerRadiusKeyFrameCollection> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this CornerRadiusAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new CornerRadiusAnimationUsingKeyFrames Clone() {
            return (CornerRadiusAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new CornerRadiusAnimationUsingKeyFrames();
        }
        #endregion
    }
}
