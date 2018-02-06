using System.Windows;

namespace DotLogix.UI.Animations {
    public sealed class CornerRadiusAnimationHelper : IAnimationHelper<CornerRadius> {
        private static readonly DoubleAnimationHelper _helper = SingletonOf<DoubleAnimationHelper>.Instance;

        #region IAnimationHelper
        public bool IsValidValue(CornerRadius value) {
            return _helper.IsValidValue(value.TopLeft) && _helper.IsValidValue(value.TopRight) &&
                   _helper.IsValidValue(value.BottomRight) && _helper.IsValidValue(value.BottomLeft);
        }

        public CornerRadius GetZeroValue() {
            return new CornerRadius(0);
        }

        public CornerRadius AddValues(CornerRadius value1, CornerRadius value2) {
            return new CornerRadius(_helper.AddValues(value1.TopLeft, value2.TopLeft),
                                    _helper.AddValues(value1.TopRight, value2.TopRight),
                                    _helper.AddValues(value1.BottomRight, value2.BottomRight),
                                    _helper.AddValues(value1.BottomLeft, value2.BottomLeft));
        }

        public CornerRadius SubtractValue(CornerRadius value1, CornerRadius value2) {
            return new CornerRadius(_helper.SubtractValue(value1.TopLeft, value2.TopLeft),
                                    _helper.SubtractValue(value1.TopRight, value2.TopRight),
                                    _helper.SubtractValue(value1.BottomRight, value2.BottomRight),
                                    _helper.SubtractValue(value1.BottomLeft, value2.BottomLeft));
        }

        public CornerRadius ScaleValue(CornerRadius value, double factor) {
            return new CornerRadius(_helper.ScaleValue(value.TopLeft, factor),
                                    _helper.ScaleValue(value.TopRight, factor),
                                    _helper.ScaleValue(value.BottomRight, factor),
                                    _helper.ScaleValue(value.BottomLeft, factor));
        }

        public CornerRadius InterpolateValue(CornerRadius from, CornerRadius to, double progress) {
            return new CornerRadius(_helper.InterpolateValue(from.TopLeft, to.TopLeft, progress),
                                    _helper.InterpolateValue(from.TopRight, to.TopRight, progress),
                                    _helper.InterpolateValue(from.BottomRight, to.BottomRight, progress),
                                    _helper.InterpolateValue(from.BottomLeft, to.BottomLeft, progress));
        }

        public double GetSegmentLength(CornerRadius from, CornerRadius to) {
            return _helper.GetSegmentLength(from.TopLeft, to.TopLeft) +
                   _helper.GetSegmentLength(from.TopRight, to.TopRight) +
                   _helper.GetSegmentLength(from.BottomRight, to.BottomRight) +
                   _helper.GetSegmentLength(from.BottomLeft, to.BottomLeft);
        }

        bool IAnimationHelper<CornerRadius>.IsAccumulable {
            get { return true; }
        }
        #endregion IAnimationHelper
    }
}