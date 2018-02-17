// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EasingKeyFrame.cs
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
    ///     Animates from Value of the previous key frame to the Value of this key frame using linear interpolation with easing
    ///     function.
    /// </summary>
    public class EasingKeyFrame<TValue, TAnimationHelper> :
    LinearKeyFrame<TValue, TAnimationHelper>
        where TAnimationHelper : class, IAnimationHelper<TValue>, new() {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new EasingKeyFrame<TValue, TAnimationHelper>();
        }
        #endregion

        #region KeyFrame
        protected override TValue InterpolateValueCore(TValue baseValue, double keyFrameProgress) {
            // modify progress with easing function
            var easingFunction = EasingFunction;
            if(easingFunction != null)
                keyFrameProgress = easingFunction.Ease(keyFrameProgress);

            // call base for linear interpolation
            return base.InterpolateValueCore(baseValue, keyFrameProgress);
        }
        #endregion

        #region Constructors
        static EasingKeyFrame() {
            EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction),
                                                                 typeof(EasingKeyFrame<TValue, TAnimationHelper>));
        }

        public EasingKeyFrame() { }
        public EasingKeyFrame(TValue value) : base(value) { }
        public EasingKeyFrame(TValue value, KeyTime keyTime) : base(value, keyTime) { }

        public EasingKeyFrame(TValue value, KeyTime keyTime, IEasingFunction easingFunction) : base(value, keyTime) {
            EasingFunction = easingFunction;
        }
        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty EasingFunctionProperty;

        /// <summary>
        ///     Defines modifier of the animation progress.
        /// </summary>
        public IEasingFunction EasingFunction {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }
        #endregion
    }
}
