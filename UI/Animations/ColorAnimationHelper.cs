// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ColorAnimationHelper.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public class ColorAnimationHelper : IAnimationHelper<Color> {
        #region IAnimationHelper
        public bool IsValidValue(Color value) {
            return true;
        }

        public Color GetZeroValue() {
            return Color.FromScRgb(0.0F, 0.0F, 0.0F, 0.0F);
        }

        public Color AddValues(Color value1, Color value2) {
            return value1 + value2;
        }

        public Color SubtractValue(Color value1, Color value2) {
            return value1 - value2;
        }

        public Color ScaleValue(Color value, double factor) {
            return value * (float)factor;
        }

        public Color InterpolateValue(Color from, Color to, double progress) {
            return from + ((to - from) * (float)progress);
        }

        public double GetSegmentLength(Color from, Color to) {
            return Math.Abs(to.ScA - from.ScA)
                   + Math.Abs(to.ScR - from.ScR)
                   + Math.Abs(to.ScG - from.ScG)
                   + Math.Abs(to.ScB - from.ScB);
        }

        public bool IsAccumulable {
            get { return true; }
        }
        #endregion
    }
}
