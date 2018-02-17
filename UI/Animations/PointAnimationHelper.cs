// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PointAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    public class PointAnimationHelper : IAnimationHelper<Point> {
        #region IAnimationHelper
        public bool IsValidValue(Point value) {
            return DoubleAnimationHelper.IsValid(value.X) && DoubleAnimationHelper.IsValid(value.Y);
        }

        public Point GetZeroValue() {
            return new Point();
        }

        public Point AddValues(Point value1, Point value2) {
            return new Point(
                             value1.X + value2.X,
                             value1.Y + value2.Y);
        }

        public Point SubtractValue(Point value1, Point value2) {
            return new Point(
                             value1.X - value2.X,
                             value1.Y - value2.Y);
        }

        public Point ScaleValue(Point value, double factor) {
            return new Point(
                             value.X * factor,
                             value.Y * factor);
        }

        public Point InterpolateValue(Point from, Point to, double progress) {
            return from + ((to - from) * progress);
        }

        public double GetSegmentLength(Point from, Point to) {
            return Math.Abs((to - from).Length);
        }

        public bool IsAccumulable {
            get { return true; }
        }
        #endregion
    }
}
