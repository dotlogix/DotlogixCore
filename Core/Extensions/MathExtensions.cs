// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MathExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

using System;

namespace DotLogix.Core.Extensions {
    public static class MathExtensions {
        #region Clamp
        /// <summary>
        ///     Checks if a value is between min and max value
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> {
            if(value.CompareTo(min) < 0)
                return min;
            if(value.CompareTo(max) > 0)
                return max;
            return value;
        }
        #endregion

        #region LaysBetween
        /// <summary>
        ///     Checks if a value is between min and max value
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static bool LaysBetween<T>(this T value, T min, T max) where T : IComparable<T> {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }
        #endregion

        #region IsPowerOfTwo
        /// <summary>
        ///     Checks if a value is a power of two
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(this int value) {
            if(value < 0)
                value = -value;
            return (value != 0) && ((value & -value) == value);
        }

        /// <summary>
        ///     Checks if a value is a power of two
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(this uint value) {
            return (value != 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        ///     Checks if a value is a power of two
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(this long value) {
            if(value < 0)
                value = -value;
            return (value != 0) && ((value & -value) == value);
        }

        /// <summary>
        ///     Checks if a value is a power of two
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(this ulong value) {
            return (value != 0) && ((value & (value - 1)) == 0);
        }
        #endregion
    }
}
