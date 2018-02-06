// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DoubleAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
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

        bool IAnimationHelper<double>.IsAccumulable {
            get { return true; }
        }
        #endregion IAnimationHelper
    }
}
