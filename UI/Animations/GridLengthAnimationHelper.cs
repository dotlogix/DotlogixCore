using System;
using System.Windows;

namespace DotLogix.UI.Animations {
    public sealed class GridLengthAnimationHelper : IAnimationHelper<GridLength> {
        private static GridUnitType VerifyCompatibility(GridLength value1, GridLength value2) {
            if(value2.Value.CompareTo(0.0) == 0)
                return value1.GridUnitType;
            if(value1.Value.CompareTo(0.0) == 0)
                return value2.GridUnitType;
            if(value1.GridUnitType != value2.GridUnitType)
                throw new InvalidOperationException("Using of different GridLengs types");
            return value1.GridUnitType;
        }

        #region IAnimationHelper
        public bool IsValidValue(GridLength value) {
            return !value.IsAuto;
        }

        public GridLength GetZeroValue() {
            return new GridLength(0);
        }

        public GridLength AddValues(GridLength value1, GridLength value2) {
            var targetType = VerifyCompatibility(value1, value2);
            return new GridLength(value1.Value + value2.Value, targetType);
        }

        public GridLength SubtractValue(GridLength value1, GridLength value2) {
            var targetType = VerifyCompatibility(value1, value2);
            return new GridLength(value1.Value - value2.Value, targetType);
        }

        public GridLength ScaleValue(GridLength value, double factor) {
            if(value.IsAuto)
                throw new InvalidOperationException("Cannot animate GridLengs with Auto type");
            return new GridLength(value.Value * factor, value.GridUnitType);
        }

        public GridLength InterpolateValue(GridLength from, GridLength to, double progress) {
            var targetType = VerifyCompatibility(from, to);
            return new GridLength(from.Value + ((to.Value - from.Value) * progress), targetType);
        }

        public double GetSegmentLength(GridLength from, GridLength to) {
            VerifyCompatibility(from, to);
            return Math.Abs(to.Value - from.Value);
        }

        bool IAnimationHelper<GridLength>.IsAccumulable {
            get { return true; }
        }
        #endregion IAnimationHelper
    }
}