﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MathExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Extensions {
    public static class MathExtension {
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static byte Clamp(this byte value, byte min, byte max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static sbyte Clamp(this sbyte value, sbyte min, sbyte max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static short Clamp(this short value, short min, short max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static ushort Clamp(this ushort value, ushort min, ushort max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static int Clamp(this int value, int min, int max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static uint Clamp(this uint value, uint min, uint max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static long Clamp(this long value, long min, long max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static ulong Clamp(this ulong value, ulong min, ulong max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static float Clamp(this float value, float min, float max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
        /// <summary>
        /// Clamps a value between min and max value
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns></returns>
        public static double Clamp(this double value, double min, double max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        /// <summary>
        /// Checks if a value is a power of two
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(this int value) {
            if(value < 0)
                value = -value;
            return (value != 0) && ((value & -value) == value);
        }

        /// <summary>
        /// Checks if a value is a power of two
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(this uint value) {
            return (value != 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Checks if a value is a power of two
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(this long value) {
            if(value < 0)
                value = -value;
            return (value != 0) && ((value & -value) == value);
        }

        /// <summary>
        /// Checks if a value is a power of two
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(this ulong value) {
            return (value != 0) && ((value & (value - 1)) == 0);
        }
    }
}
