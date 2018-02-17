// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Animation.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;
#endregion

// ReSharper disable StaticFieldInGenericType

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Generic animation class for TValue type. Uses TAnimationHelper to animate value. For using in XAML subclass it with
    ///     non-generic class.
    /// </summary>
    public class Animation<TValue, TNullableValue, TAnimationHelper> : AnimationBase<TValue>
        where TAnimationHelper : class, IAnimationHelper<TValue>, new() {
        #region Constructors
        static Animation() {
            NullableHelper.ValidateTypesCompatibility<TValue, TNullableValue>();
            NullableHelper.ValidateNullable<TNullableValue>();

            var ownerType = typeof(Animation<TValue, TNullableValue, TAnimationHelper>);

            EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), ownerType);

            var propertyType = typeof(TNullableValue);

            FromProperty = DependencyProperty.Register("From", propertyType, ownerType,
                                                       new PropertyMetadata(null, OnValuePropertyChanged),
                                                       OnValidateValue);

            ToProperty = DependencyProperty.Register("To", propertyType, ownerType,
                                                     new PropertyMetadata(null, OnValuePropertyChanged),
                                                     OnValidateValue);

            ByProperty = DependencyProperty.Register("By", propertyType, ownerType,
                                                     new PropertyMetadata(null, OnValuePropertyChanged),
                                                     OnValidateValue);
        }
        #endregion

        #region AnimationBase
        protected override TValue GetCurrentValueCore(TValue defaultOriginValue, TValue defaultDestinationValue,
                                                      AnimationClock animationClock) {
            if(_lane == Lane.Invalid)
                ValidateLane();

            var from = AnimationHelper.GetZeroValue();
            var to = AnimationHelper.GetZeroValue();
            var originValue = AnimationHelper.GetZeroValue();

            const string defaultOriginValueName = "defaultOriginValue";
            const string defaultDestinationValueName = "defaultDestinationValue";

            switch(_lane) {
                case Lane.Default:
                    ValidateValue(defaultOriginValue, defaultOriginValueName);
                    ValidateValue(defaultDestinationValue, defaultDestinationValueName);
                    from = defaultOriginValue;
                    to = defaultDestinationValue;

                    break;

                case Lane.FromOnly:
                    ValidateValue(defaultDestinationValue, defaultDestinationValueName);
                    from = _cache[0];
                    to = defaultDestinationValue;
                    break;

                case Lane.ToOnly:
                    ValidateValue(defaultOriginValue, defaultOriginValueName);
                    from = defaultOriginValue;
                    to = _cache[0];
                    break;

                case Lane.ByOnly:
                    ValidateValue(defaultOriginValue, defaultOriginValueName);
                    to = _cache[0];
                    if(AnimationHelper.IsAccumulable)
                        originValue = defaultOriginValue;
                    break;

                case Lane.FromTo:
                    ValidateValue(defaultOriginValue, defaultOriginValueName);
                    from = _cache[0];
                    to = _cache[1];

                    if(IsAdditive && AnimationHelper.IsAccumulable) {
                        ValidateValue(defaultOriginValue, defaultOriginValueName);
                        originValue = defaultOriginValue;
                    }
                    break;

                case Lane.FromBy:

                    from = _cache[0];
                    to = AnimationHelper.AddValues(_cache[0], _cache[1]);

                    if(IsAdditive && AnimationHelper.IsAccumulable) {
                        ValidateValue(defaultOriginValue, defaultOriginValueName);
                        originValue = defaultOriginValue;
                    }
                    break;

                default:
                    Debug.Fail("Invalid animation lane");
                    break;
            }

            if(IsCumulative && AnimationHelper.IsAccumulable) {
                var currentRepeat = (double)(animationClock.CurrentIteration.GetValueOrDefault() - 1);
                if(currentRepeat > 0.0) {
                    var range = AnimationHelper.SubtractValue(to, from);
                    originValue =
                    AnimationHelper.AddValues(originValue, AnimationHelper.ScaleValue(range, currentRepeat));
                }
            }

            var progress = animationClock.CurrentProgress.GetValueOrDefault();
            var easingFunction = EasingFunction;
            if(easingFunction != null)
                progress = easingFunction.Ease(progress);

            return AnimationHelper.AddValues(originValue, AnimationHelper.InterpolateValue(from, to, progress));
        }
        #endregion

        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new Animation<TValue, TNullableValue, TAnimationHelper>();
        }
        #endregion

        #region Inner types
        private enum Lane : byte {
            Invalid,
            Default,
            FromOnly,
            ToOnly,
            ByOnly,
            FromTo,
            FromBy
        }
        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty FromProperty;
        public static readonly DependencyProperty ToProperty;
        public static readonly DependencyProperty ByProperty;
        public static readonly DependencyProperty EasingFunctionProperty;

        /// <summary>
        ///     Defines Animation starting value
        /// </summary>
        public TNullableValue From {
            get => (TNullableValue)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <summary>
        ///     Defines Animation ending value
        /// </summary>
        public TNullableValue To {
            get => (TNullableValue)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        /// <summary>
        ///     Defines Animation step value
        /// </summary>
        public TNullableValue By {
            get => (TNullableValue)GetValue(ByProperty);
            set => SetValue(ByProperty, value);
        }

        /// <summary>
        ///     Defines modifier of the animation progress.
        /// </summary>
        public IEasingFunction EasingFunction {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        /// <summary>
        ///     If this property is set to true base value will be incremented by the the current animation value instead of
        ///     replacing it entirely.
        /// </summary>
        public bool IsAdditive {
            get => (bool)GetValue(IsAdditiveProperty);
            set => SetValue(IsAdditiveProperty, value);
        }

        /// <summary>
        ///     It this property is set to true, the animation will accumulate its value over repeats. For instance if you have a
        ///     From value of 0.0 and a To value of 1.0, the animation return values from 1.0 to 2.0 over the second repeat cycle,
        ///     and 2.0 to 3.0 over the third, etc.
        /// </summary>
        public bool IsCumulative {
            get => (bool)GetValue(IsCumulativeProperty);
            set => SetValue(IsCumulativeProperty, value);
        }
        #endregion

        #region Service methods
        private void ValidateLane() {
            _lane = Lane.Default;
            _cache = null;

            if(From.IsNull()) {
                if(To.IsNotNull()) {
                    _lane = Lane.ToOnly;
                    _cache = new[] {(TValue)(object)To};
                } else if(By.IsNotNull()) {
                    _lane = Lane.ByOnly;
                    _cache = new[] {(TValue)(object)By};
                }
            } else if(To.IsNotNull()) {
                _lane = Lane.FromTo;
                _cache = new[] {(TValue)(object)From, (TValue)(object)To};
            } else if(By.IsNotNull()) {
                _lane = Lane.FromBy;
                _cache = new[] {(TValue)(object)From, (TValue)(object)By};
            } else {
                _lane = Lane.FromOnly;
                _cache = new[] {(TValue)(object)From};
            }
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((Animation<TValue, TNullableValue, TAnimationHelper>)d)._lane = Lane.Invalid;
        }

        private static bool OnValidateValue(object value) {
            var nullableValue = (TNullableValue)value;
            return nullableValue.IsNull() || AnimationHelper.IsValidValue((TValue)(object)nullableValue);
        }

        private static void ValidateValue(TValue value, string name) {
            if(!AnimationHelper.IsValidValue(value))
                throw new InvalidOperationException("Animation: Invalid " + name);
        }
        #endregion

        #region Fields
        private Lane _lane = Lane.Invalid;
        private TValue[] _cache;

        protected static readonly IAnimationHelper<TValue> AnimationHelper = SingletonOf<TAnimationHelper>.Instance;
        #endregion
    }
}

// ReSharper restore StaticFieldInGenericType
