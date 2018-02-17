// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LinearGradientBrushAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Diagnostics;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public class LinearGradientBrushAnimationHelper : GradientBrushAnimationHelper<LinearGradientBrush> {
        private static readonly PointAnimationHelper _pointHelper = SingletonOf<PointAnimationHelper>.Instance;

        protected override LinearGradientBrush Create() {
            return new LinearGradientBrush();
        }

        protected new LinearGradientBrush VerifyCompatibility(LinearGradientBrush value1, LinearGradientBrush value2) {
            if(value1.GradientStops.Count == 0)
                return value2;
            if(value2.GradientStops.Count == 0)
                return value1;
            base.VerifyCompatibility(value1, value2);
            return null;
        }

        #region IAnimationHelper
        public override bool IsValidValue(LinearGradientBrush value) {
            return true;
        }

        public override LinearGradientBrush GetZeroValue() {
            return new LinearGradientBrush();
        }

        public override LinearGradientBrush AddValues(LinearGradientBrush value1, LinearGradientBrush value2) {
            var target = VerifyCompatibility(value1, value2);
            if(target != null)
                return target;

            var brush = base.AddValues(value1, value2);
            brush.StartPoint = _pointHelper.AddValues(value1.StartPoint, value2.StartPoint);
            brush.EndPoint = _pointHelper.AddValues(value1.EndPoint, value2.EndPoint);
            return brush;
        }

        public override LinearGradientBrush SubtractValue(LinearGradientBrush value1, LinearGradientBrush value2) {
            var target = VerifyCompatibility(value1, value2);
            if(target != null) {
                Debug.Assert(target == value1);
                return target;
            }
            var brush = base.SubtractValue(value1, value2);
            brush.StartPoint = _pointHelper.SubtractValue(value1.StartPoint, value2.StartPoint);
            brush.EndPoint = _pointHelper.SubtractValue(value1.EndPoint, value2.EndPoint);
            return brush;
        }

        public override LinearGradientBrush ScaleValue(LinearGradientBrush value, double factor) {
            var brush = base.ScaleValue(value, factor);
            brush.StartPoint = _pointHelper.ScaleValue(value.StartPoint, factor);
            brush.EndPoint = _pointHelper.ScaleValue(value.EndPoint, factor);
            return brush;
        }

        public override LinearGradientBrush InterpolateValue(LinearGradientBrush from, LinearGradientBrush to,
                                                             double progress) {
            var target = VerifyCompatibility(from, to);
            if(target != null) {
                Debug.Assert(target == to);
                return ScaleValue(to, progress);
            }
            var brush = base.InterpolateValue(from, to, progress);
            brush.StartPoint = _pointHelper.InterpolateValue(from.StartPoint, to.StartPoint, progress);
            brush.EndPoint = _pointHelper.InterpolateValue(from.EndPoint, to.EndPoint, progress);
            return brush;
        }

        public override double GetSegmentLength(LinearGradientBrush from, LinearGradientBrush to) {
            if(VerifyCompatibility(from, to) != null)
                return 1.0;
            return from != to ? 1.0 : 0.0;
        }
        #endregion IAnimationHelper
    }
}
