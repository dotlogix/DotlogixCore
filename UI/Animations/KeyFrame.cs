// ==================================================
// Copyright 2018(C) , DotLogix
// File:  KeyFrame.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Windows;
using System.Windows.Media.Animation;
#endregion

namespace DotLogix.UI.Animations {
    // ReSharper disable StaticFieldInGenericType
    public abstract class KeyFrame<TValue> : Freezable, IKeyFrame {
        #region Public Methods
        /// <summary>
        ///     Returns the interpolated value of a this key frame at the progress increment provided. Used by
        ///     AnimationUsingKeyFrames.
        /// </summary>
        public TValue InterpolateValue(TValue baseValue, double keyFrameProgress) {
            if((keyFrameProgress < 0.0) || (keyFrameProgress > 1.0))
                throw new ArgumentOutOfRangeException(nameof(keyFrameProgress));

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }
        #endregion

        /// <summary>
        ///     Override this method to calculate the value of this key frame at the progress increment provided.
        /// </summary>
        protected abstract TValue InterpolateValueCore(TValue baseValue, double keyFrameProgress);

        #region Constructors
        static KeyFrame() {
            var ownerType = typeof(KeyFrame<TValue>);
            KeyTimeProperty = DependencyProperty.Register("KeyTime", typeof(KeyTime), ownerType,
                                                          new PropertyMetadata(KeyTime.Uniform));
            ValueProperty = DependencyProperty.Register("Value", typeof(TValue), ownerType);
        }

        protected KeyFrame() { }

        protected KeyFrame(TValue value) {
            Value = value;
        }

        protected KeyFrame(TValue value, KeyTime keyTime) : this(value) {
            KeyTime = keyTime;
        }
        #endregion

        #region DependencyProperty
        public static readonly DependencyProperty KeyTimeProperty;
        public static readonly DependencyProperty ValueProperty;

        /// <summary>
        ///     The value that associated animation should reach at KeyTime.
        /// </summary>
        public TValue Value {
            get => (TValue)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        #region IKeyFrame
        /// <summary>
        ///     The time at which associated animation value should be equal to the Value property.
        /// </summary>
        public KeyTime KeyTime {
            get => (KeyTime)GetValue(KeyTimeProperty);
            set => SetValue(KeyTimeProperty, value);
        }

        object IKeyFrame.Value {
            get => Value;
            set => Value = (TValue)value;
        }
        #endregion
        #endregion
    }

    // ReSharper restore StaticFieldInGenericType
}
