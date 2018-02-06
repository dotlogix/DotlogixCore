// ==================================================
// Copyright 2016(C) , DotLogix
// File:  AnimationBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
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
                throw new ArgumentNullException("defaultOriginValue");
            if(defaultDestinationValue == null)
                throw new ArgumentNullException("defaultDestinationValue");
            if(animationClock == null)
                throw new ArgumentNullException("animationClock");

            ReadPreamble();
            if(animationClock.CurrentState == ClockState.Stopped)
                return defaultDestinationValue;
            return GetCurrentValueCore((TValue)defaultOriginValue, (TValue)defaultDestinationValue, animationClock);
        }

        public sealed override Type TargetPropertyType {
            get { return typeof(TValue); }
        }
        #endregion
    }
}
