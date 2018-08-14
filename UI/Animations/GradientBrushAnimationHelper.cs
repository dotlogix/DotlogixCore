// ==================================================
// Copyright 2018(C) , DotLogix
// File:  GradientBrushAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public abstract class GradientBrushAnimationHelper<TValue> : IAnimationHelper<TValue> where TValue : GradientBrush {
        protected abstract TValue Create();

        private void CopyProperties(TValue src, TValue target) {
            target.SpreadMethod = src.SpreadMethod;
            target.MappingMode = src.MappingMode;
            target.ColorInterpolationMode = src.ColorInterpolationMode;
        }

        protected void VerifyCompatibility(TValue value1, TValue value2) {
            if(value1.SpreadMethod != value2.SpreadMethod)
                throw new InvalidOperationException("SpreadMethod should be the same for brushes");
            if(value1.MappingMode != value2.MappingMode)
                throw new InvalidOperationException("MappingMode should be the same for brushes");
            if(value1.ColorInterpolationMode != value2.ColorInterpolationMode)
                throw new InvalidOperationException("ColorInterpolationMode should be the same for brushes");
            if(value1.GradientStops.Count != value2.GradientStops.Count)
                throw new InvalidOperationException("Gradient stops count should be the same for brushes");
        }

        // ReSharper disable StaticFieldInGenericType
        protected static readonly DoubleAnimationHelper DoubleHelper = SingletonOf<DoubleAnimationHelper>.Instance;

        protected static readonly ColorAnimationHelper ColorHelper = SingletonOf<ColorAnimationHelper>.Instance;
        // ReSharper restore StaticFieldInGenericType

        #region IAnimationHelper
        public virtual bool IsValidValue(TValue value) {
            return true;
        }

        public abstract TValue GetZeroValue();

        public virtual TValue AddValues(TValue value1, TValue value2) {
            var brush = Create();
            CopyProperties(value1, brush);
            // calc gradient stops
            for(var cnt = 0; cnt < value1.GradientStops.Count; cnt++) {
                var stop1 = value1.GradientStops[cnt];
                var stop2 = value2.GradientStops[cnt];

                var color = ColorHelper.AddValues(stop1.Color, stop2.Color);
                var offset = DoubleHelper.AddValues(stop1.Offset, stop2.Offset);

                brush.GradientStops.Add(new GradientStop(color, offset));
            }
            return brush;
        }

        public virtual TValue SubtractValue(TValue value1, TValue value2) {
            var brush = Create();
            CopyProperties(value1, brush);
            for(var cnt = 0; cnt < value1.GradientStops.Count; cnt++) {
                var stop1 = value1.GradientStops[cnt];
                var stop2 = value2.GradientStops[cnt];

                var color = ColorHelper.SubtractValue(stop1.Color, stop2.Color);
                var offset = DoubleHelper.SubtractValue(stop1.Offset, stop2.Offset);

                brush.GradientStops.Add(new GradientStop(color, offset));
            }
            return brush;
        }

        public virtual TValue ScaleValue(TValue value, double factor) {
            var brush = Create();
            CopyProperties(value, brush);
            for(var cnt = 0; cnt < value.GradientStops.Count; cnt++) {
                var stop = value.GradientStops[cnt];

                var color = ColorHelper.ScaleValue(stop.Color, factor);
                var offset = DoubleHelper.ScaleValue(stop.Offset, factor);

                brush.GradientStops.Add(new GradientStop(color, offset));
            }
            return brush;
        }

        public virtual TValue InterpolateValue(TValue from, TValue to, double progress) {
            var brush = Create();
            CopyProperties(from, brush);
            for(var cnt = 0; cnt < from.GradientStops.Count; cnt++) {
                var stop1 = from.GradientStops[cnt];
                var stop2 = to.GradientStops[cnt];

                var color = ColorHelper.InterpolateValue(stop1.Color, stop2.Color, progress);
                var offset = DoubleHelper.InterpolateValue(stop1.Offset, stop2.Offset, progress);

                brush.GradientStops.Add(new GradientStop(color, offset));
            }
            return brush;
        }

        public abstract double GetSegmentLength(TValue from, TValue to);

        public virtual bool IsAccumulable => true;
        #endregion IAnimationHelper
    }
}
