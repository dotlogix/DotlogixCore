// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CornerRadiusAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    public sealed class CornerRadiusAnimationHelper : IAnimationHelper<CornerRadius> {
        private static readonly DoubleAnimationHelper Helper = SingletonOf<DoubleAnimationHelper>.Instance;

        #region IAnimationHelper
        public bool IsValidValue(CornerRadius value) {
            return Helper.IsValidValue(value.TopLeft) && Helper.IsValidValue(value.TopRight) &&
                   Helper.IsValidValue(value.BottomRight) && Helper.IsValidValue(value.BottomLeft);
        }

        public CornerRadius GetZeroValue() {
            return new CornerRadius(0);
        }

        public CornerRadius AddValues(CornerRadius value1, CornerRadius value2) {
            return new CornerRadius(Helper.AddValues(value1.TopLeft, value2.TopLeft),
                                    Helper.AddValues(value1.TopRight, value2.TopRight),
                                    Helper.AddValues(value1.BottomRight, value2.BottomRight),
                                    Helper.AddValues(value1.BottomLeft, value2.BottomLeft));
        }

        public CornerRadius SubtractValue(CornerRadius value1, CornerRadius value2) {
            return new CornerRadius(Helper.SubtractValue(value1.TopLeft, value2.TopLeft),
                                    Helper.SubtractValue(value1.TopRight, value2.TopRight),
                                    Helper.SubtractValue(value1.BottomRight, value2.BottomRight),
                                    Helper.SubtractValue(value1.BottomLeft, value2.BottomLeft));
        }

        public CornerRadius ScaleValue(CornerRadius value, double factor) {
            return new CornerRadius(Helper.ScaleValue(value.TopLeft, factor),
                                    Helper.ScaleValue(value.TopRight, factor),
                                    Helper.ScaleValue(value.BottomRight, factor),
                                    Helper.ScaleValue(value.BottomLeft, factor));
        }

        public CornerRadius InterpolateValue(CornerRadius from, CornerRadius to, double progress) {
            return new CornerRadius(Helper.InterpolateValue(from.TopLeft, to.TopLeft, progress),
                                    Helper.InterpolateValue(from.TopRight, to.TopRight, progress),
                                    Helper.InterpolateValue(from.BottomRight, to.BottomRight, progress),
                                    Helper.InterpolateValue(from.BottomLeft, to.BottomLeft, progress));
        }

        public double GetSegmentLength(CornerRadius from, CornerRadius to) {
            return Helper.GetSegmentLength(from.TopLeft, to.TopLeft) +
                   Helper.GetSegmentLength(from.TopRight, to.TopRight) +
                   Helper.GetSegmentLength(from.BottomRight, to.BottomRight) +
                   Helper.GetSegmentLength(from.BottomLeft, to.BottomLeft);
        }

        bool IAnimationHelper<CornerRadius>.IsAccumulable => true;
        #endregion IAnimationHelper
    }
}
