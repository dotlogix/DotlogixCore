// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MathExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

using System;

namespace DotLogix.Core.Extensions; 

/// <summary>
/// A static class providing extension methods for numbers/>
/// </summary>
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

    /// <summary>
    ///     Checks if a value is between min and max value
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <param name="range">The range</param>
    /// <returns></returns>
    public static bool LaysBetween(this int value, Utils.Patterns.Range range) {
        if(range.Min.HasValue && value < range.Min.Value)
            return false;
        if (range.Max.HasValue && value > range.Max.Value)
            return false;
        return true;
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
    ///     Rounds the value to the next bigger power of two
    /// </summary>
    public static uint CeilPowerOfTwo(this uint value) {
        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value++;
        return value;
    }
        
    /// <summary>
    ///     Rounds the value to the next bigger power of two
    /// </summary>
    public static ulong CeilPowerOfTwo(this ulong value) {
        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value |= value >> 32;
        value++;
        return value;
    }
        
    /// <summary>
    ///     Counts the amount of leading zeros of an unsigned integer
    /// </summary>
    public static int CountLeadingZeros(this uint input)
    {
        if (input == 0) return 32;
        var n = 1;
        if ((input >> 16) == 0) { n += 16; input <<= 16; }
        if ((input >> 24) == 0) { n += 8; input <<= 8; }
        if ((input >> 28) == 0) { n += 4; input <<= 4; }
        if ((input >> 30) == 0) { n += 2; input <<= 2; }
        n -= (int)(input >> 31);
        return n;
    }
        
    /// <summary>
    ///     Counts the amount of leading zeros of an unsigned long
    /// </summary>
    public static int CountLeadingZeros(this ulong input)
    {
        if (input == 0) return 64;
        var n = 1;
        if ((input >> 32) == 0) { n += 32; input <<= 32; }
        if ((input >> 48) == 0) { n += 16; input <<= 16; }
        if ((input >> 56) == 0) { n += 8; input <<= 8; }
        if ((input >> 60) == 0) { n += 4; input <<= 4; }
        if ((input >> 62) == 0) { n += 2; input <<= 2; }
        n -= (int)(input >> 63);
        return n;
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

    private static readonly uint[] PowersOf10 = {
        1,
        10,
        100,
        1000,
        10000,
        100000,
        1000000,
        10000000,
        100000000,
        1000000000
    };

    private static readonly ulong[] PowersOf10L = {
        1,
        10,
        100,
        1000,
        10000,
        100000,
        1000000,
        10000000,
        100000000,
        1000000000,
        10000000000,
        100000000000,
        1000000000000,
        10000000000000,
        100000000000000,
        1000000000000000,
        10000000000000000,
        100000000000000000,
        1000000000000000000,
        10000000000000000000
    };



    public static int DigitCount(this byte value) {
        return DigitCount((uint)value);
    }

    public static int DigitCount(this sbyte value) {
        return DigitCount((int)value);
    }

    public static int DigitCount(this ushort value) {
        return DigitCount((uint)value);
    }

    public static int DigitCount(this short value) {
        return DigitCount((int)value);
    }

    public static int DigitCount(this int value) {
        if (value == 0)
            return 1;

        var hasSign = value < 0;
        if (hasSign) {
            value = -value;
        }

        var length = DigitCount((uint)value);
        return hasSign ? length + 1 : length;
    }

    public static int DigitCount(this uint value) {
        if (value == 0)
            return 1;

        for (var i = 0; i < PowersOf10.Length; i++) {
            if (value < PowersOf10[i])
                return i;
        }

        return PowersOf10.Length;
    }


    public static int DigitCount(this long value)
    {
        if (value == 0)
            return 1;

        var hasSign = value < 0;
        if (hasSign) {
            value = -value;
        }

        var length = DigitCount((ulong)value);
        return hasSign ? length + 1 : length;
    }

    public static int DigitCount(this ulong value) {
        if (value == 0)
            return 1;

        if(value >= PowersOf10L[10])
        {
            for (var i = 11; i < PowersOf10L.Length; i++)
            {
                if (value < PowersOf10L[i])
                    return i;
            }
            return PowersOf10L.Length;
        }

        for (var i = 0; i < 10; i++) {
            if (value < PowersOf10L[i])
                return i;
        }
        return 10;
    }



}