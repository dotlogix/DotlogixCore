// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SplineKeyFrame.cs
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
    ///     Animates from Value of the previous key frame to the Value of this key frame using linear interpolation with
    ///     KeySpline function.
    /// </summary>
    public class SplineKeyFrame<TValue, TAnimationHelper> :
    LinearKeyFrame<TValue, TAnimationHelper>
        where TAnimationHelper : class, IAnimationHelper<TValue>, new() {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new SplineKeyFrame<TValue, TAnimationHelper>();
        }
        #endregion

        #region KeyFrame
        protected override TValue InterpolateValueCore(TValue baseValue, double keyFrameProgress) {
            // modify progress with KeySpline function
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if((keyFrameProgress != 0.0) && (keyFrameProgress != 1.0))
            // ReSharper restore CompareOfFloatsByEqualityOperator
                keyFrameProgress = KeySpline.GetSplineProgress(keyFrameProgress);

            // call base for linear interpolation
            return base.InterpolateValueCore(baseValue, keyFrameProgress);
        }
        #endregion

        #region Constructors
        static SplineKeyFrame() {
            KeySplineProperty = DependencyProperty.Register("KeySpline", typeof(KeySpline),
                                                            typeof(SplineKeyFrame<TValue, TAnimationHelper>),
                                                            new PropertyMetadata(new KeySpline()));
        }

        public SplineKeyFrame() { }
        public SplineKeyFrame(TValue value) : base(value) { }

        public SplineKeyFrame(TValue value, KeyTime keyTime, KeySpline keySpline = null) : base(value, keyTime) {
            if(keySpline != null)
                KeySpline = keySpline;
        }
        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty KeySplineProperty;

        /// <summary>
        ///     Defines modifier of the animation progress.
        /// </summary>
        public KeySpline KeySpline {
            get => (KeySpline)GetValue(KeySplineProperty);
            set => SetValue(KeySplineProperty, value ?? new KeySpline());
        }
        #endregion
    }
}
