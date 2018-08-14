// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DoubleAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.UI.Animations {
    public sealed class DoubleAnimationHelper : IAnimationHelper<double> {
        public static bool IsValid(double value) {
            return !double.IsInfinity(value) && !double.IsNaN(value);
        }

        #region IAnimationHelper
        public bool IsValidValue(double value) {
            return IsValid(value);
        }

        public double GetZeroValue() {
            return 0;
        }

        public double AddValues(double value1, double value2) {
            return value1 + value2;
        }

        public double SubtractValue(double value1, double value2) {
            return value1 - value2;
        }

        public double ScaleValue(double value, double factor) {
            return value * factor;
        }

        public double InterpolateValue(double from, double to, double progress) {
            return from + ((to - from) * progress);
        }

        public double GetSegmentLength(double from, double to) {
            return Math.Abs(to - from);
        }

        bool IAnimationHelper<double>.IsAccumulable => true;
        #endregion IAnimationHelper
    }
}
