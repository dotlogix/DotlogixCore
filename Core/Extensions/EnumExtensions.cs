// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EnumExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Extensions {
    public static class EnumExtensions {
        /// <summary>
        /// Converts a enum value to int
        /// </summary>
        public static int AsInt(this Enum value) {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Determines if a enum value is a single flag in a flag enum (power of two)
        /// </summary>
        public static bool IsSingleFlag(this Enum value) {
            return Convert.ToInt32(value).IsPowerOfTwo();
        }
    }
}
