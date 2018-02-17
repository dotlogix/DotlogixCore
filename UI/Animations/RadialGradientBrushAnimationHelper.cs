// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RadialGradientBrushAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Diagnostics;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public class RadialGradientBrushAnimationHelper : GradientBrushAnimationHelper<RadialGradientBrush> {
        private static readonly PointAnimationHelper _pointHelper = SingletonOf<PointAnimationHelper>.Instance;

        protected override RadialGradientBrush Create() {
            return new RadialGradientBrush();
        }

        protected new RadialGradientBrush VerifyCompatibility(RadialGradientBrush value1, RadialGradientBrush value2) {
            if(value1.GradientStops.Count == 0)
                return value2;
            if(value2.GradientStops.Count == 0)
                return value1;
            base.VerifyCompatibility(value1, value2);
            return null;
        }

        #region IAnimationHelper
        public override bool IsValidValue(RadialGradientBrush value) {
            return true;
        }

        public override RadialGradientBrush GetZeroValue() {
            return new RadialGradientBrush();
        }

        public override RadialGradientBrush AddValues(RadialGradientBrush value1, RadialGradientBrush value2) {
            var target = VerifyCompatibility(value1, value2);
            if(target != null)
                return target;

            var brush = base.AddValues(value1, value2);
            brush.Center = _pointHelper.AddValues(value1.Center, value2.Center);
            brush.GradientOrigin = _pointHelper.AddValues(value1.GradientOrigin, value2.GradientOrigin);
            brush.RadiusX = DoubleHelper.AddValues(value1.RadiusX, value2.RadiusX);
            brush.RadiusY = DoubleHelper.AddValues(value1.RadiusY, value2.RadiusY);
            return brush;
        }

        public override RadialGradientBrush SubtractValue(RadialGradientBrush value1, RadialGradientBrush value2) {
            var target = VerifyCompatibility(value1, value2);
            if(target != null) {
                Debug.Assert(target == value1);
                return target;
            }
            var brush = base.SubtractValue(value1, value2);
            brush.Center = _pointHelper.SubtractValue(value1.Center, value2.Center);
            brush.GradientOrigin = _pointHelper.SubtractValue(value1.GradientOrigin, value2.GradientOrigin);
            brush.RadiusX = DoubleHelper.SubtractValue(value1.RadiusX, value2.RadiusX);
            brush.RadiusY = DoubleHelper.SubtractValue(value1.RadiusY, value2.RadiusY);
            return brush;
        }

        public override RadialGradientBrush ScaleValue(RadialGradientBrush value, double factor) {
            var brush = base.ScaleValue(value, factor);
            brush.Center = _pointHelper.ScaleValue(value.Center, factor);
            brush.GradientOrigin = _pointHelper.ScaleValue(value.GradientOrigin, factor);
            brush.RadiusX = DoubleHelper.ScaleValue(value.RadiusX, factor);
            brush.RadiusY = DoubleHelper.ScaleValue(value.RadiusY, factor);
            return brush;
        }

        public override RadialGradientBrush InterpolateValue(RadialGradientBrush from, RadialGradientBrush to,
                                                             double progress) {
            var target = VerifyCompatibility(from, to);
            if(target != null) {
                Debug.Assert(target == to);
                return ScaleValue(to, progress);
            }
            var brush = base.InterpolateValue(from, to, progress);
            brush.Center = _pointHelper.InterpolateValue(from.Center, to.Center, progress);
            brush.GradientOrigin = _pointHelper.InterpolateValue(from.GradientOrigin, to.GradientOrigin, progress);
            brush.RadiusX = DoubleHelper.InterpolateValue(from.RadiusX, to.RadiusX, progress);
            brush.RadiusY = DoubleHelper.InterpolateValue(from.RadiusY, to.RadiusY, progress);
            return brush;
        }

        public override double GetSegmentLength(RadialGradientBrush from, RadialGradientBrush to) {
            if(VerifyCompatibility(from, to) != null)
                return 1.0;
            return from != to ? 1.0 : 0.0;
        }
        #endregion IAnimationHelper
    }
}
