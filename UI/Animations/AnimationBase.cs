// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AnimationBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Windows.Media.Animation;
#endregion

namespace DotLogix.UI.Animations {
    public abstract class AnimationBase<TValue> : AnimationTimeline {
        protected abstract TValue GetCurrentValueCore(TValue defaultOriginValue, TValue defaultDestinationValue,
                                                      AnimationClock animationClock);

        #region AnimationTimeline
        public sealed override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue,
                                                      AnimationClock animationClock) {
            if(defaultOriginValue == null)
                throw new ArgumentNullException(nameof(defaultOriginValue));
            if(defaultDestinationValue == null)
                throw new ArgumentNullException(nameof(defaultDestinationValue));
            if(animationClock == null)
                throw new ArgumentNullException(nameof(animationClock));

            ReadPreamble();
            if(animationClock.CurrentState == ClockState.Stopped)
                return defaultDestinationValue;
            return GetCurrentValueCore((TValue)defaultOriginValue, (TValue)defaultDestinationValue, animationClock);
        }

        public sealed override Type TargetPropertyType => typeof(TValue);
        #endregion
    }
}
