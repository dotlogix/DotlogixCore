﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ExpanderDoubleAnimation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Media.Animation;
#endregion

namespace DotLogix.UI.Controls {
    /// <summary>
    ///     Animates a double value
    /// </summary>
    public class ExpanderDoubleAnimation : DoubleAnimationBase {
        /// <summary>
        ///     Dependency property for the From property
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(double?),
                                                                                             typeof(
                                                                                                 ExpanderDoubleAnimation
                                                                                             ));

        /// <summary>
        ///     Dependency property for the To property
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(double?),
                                                                                           typeof(
                                                                                               ExpanderDoubleAnimation))
        ;

        /// <summary>
        ///     Sets the reverse value for the second animation
        /// </summary>
        public static readonly DependencyProperty ReverseValueProperty =
        DependencyProperty.Register("ReverseValue", typeof(double?), typeof(ExpanderDoubleAnimation),
                                    new UIPropertyMetadata(0.0));

        /// <summary>
        ///     CLR Wrapper for the From depenendency property
        /// </summary>
        public double? From {
            get => (double?)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <summary>
        ///     CLR Wrapper for the To property
        /// </summary>
        public double? To {
            get => (double?)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        /// <summary>
        ///     Sets the reverse value for the second animation
        /// </summary>
        public double? ReverseValue {
            get => (double)GetValue(ReverseValueProperty);
            set => SetValue(ReverseValueProperty, value);
        }


        /// <summary>
        ///     Creates an instance of the animation
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore() {
            return new ExpanderDoubleAnimation();
        }

        /// <summary>
        ///     Animates the double value
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate</param>
        /// <param name="defaultDestinationValue">The final value</param>
        /// <param name="animationClock">The animation clock (timer)</param>
        /// <returns>Returns the new double to set</returns>
        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue,
                                                      AnimationClock animationClock) {
            var fromVal = From.Value;
            var toVal = To.Value;

            if(defaultOriginValue == toVal) {
                fromVal = toVal;
                toVal = ReverseValue.Value;
            }

            if(fromVal > toVal) {
                return ((1 - animationClock.CurrentProgress.Value) *
                        (fromVal - toVal)) + toVal;
            }
            return (animationClock.CurrentProgress.Value *
                    (toVal - fromVal)) + fromVal;
        }
    }
}
