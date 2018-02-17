// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MathExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Extensions {
    public static class MathExtension {
        public static byte Clamp(this byte value, byte min, byte max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static sbyte Clamp(this sbyte value, sbyte min, sbyte max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static short Clamp(this short value, short min, short max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static ushort Clamp(this ushort value, ushort min, ushort max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static int Clamp(this int value, int min, int max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static uint Clamp(this uint value, uint min, uint max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static long Clamp(this long value, long min, long max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static ulong Clamp(this ulong value, ulong min, ulong max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static float Clamp(this float value, float min, float max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static double Clamp(this double value, double min, double max) {
            if(value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }

        public static bool IsPowerOfTwo(this int value) {
            if(value < 0)
                value = -value;
            return (value != 0) && ((value & -value) == value);
        }

        public static bool IsPowerOfTwo(this uint value) {
            return (value != 0) && ((value & (value - 1)) == 0);
        }

        public static bool IsPowerOfTwo(this long value) {
            if(value < 0)
                value = -value;
            return (value != 0) && ((value & -value) == value);
        }

        public static bool IsPowerOfTwo(this ulong value) {
            return (value != 0) && ((value & (value - 1)) == 0);
        }
    }
}
