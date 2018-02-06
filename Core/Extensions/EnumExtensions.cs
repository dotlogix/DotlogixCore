// ==================================================
// Copyright 2016(C) , DotLogix
// File:  EnumExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Extensions {
    public static class EnumExtensions {
        public static int AsInt(this Enum value) {
            return Convert.ToInt32(value);
        }

        public static bool IsSingleFlag(this Enum value) {
            return Convert.ToInt32(value).IsPowerOfTwo();
        }
    }
}
