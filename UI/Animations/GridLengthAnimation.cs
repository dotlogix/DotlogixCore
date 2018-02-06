// ==================================================
// Copyright 2016(C) , DotLogix
// File:  GridLengthAnimation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    public class GridLengthAnimation : ValueTypeAnimation<GridLength, GridLengthAnimationHelper> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this GridLengthAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new GridLengthAnimation Clone() {
            return (GridLengthAnimation)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new GridLengthAnimation();
        }
        #endregion
    }
}
