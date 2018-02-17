// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DiscreteKeyFrame.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Media.Animation;
#endregion

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Animates from Value of the previous key frame to the Value of this key frame without interpolation. The change
    ///     occurs at the KeyTime.
    /// </summary>
    public class DiscreteKeyFrame<TValue> : KeyFrame<TValue> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new DiscreteKeyFrame<TValue>();
        }
        #endregion

        #region KeyFrame
        protected override TValue InterpolateValueCore(TValue baseValue, double keyFrameProgress) {
            return keyFrameProgress < 1.0 ? baseValue : Value;
        }
        #endregion

        #region Constructors
        public DiscreteKeyFrame() { }
        public DiscreteKeyFrame(TValue value) : base(value) { }
        public DiscreteKeyFrame(TValue value, KeyTime keyTime) : base(value, keyTime) { }
        #endregion
    }
}
