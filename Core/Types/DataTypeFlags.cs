// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DataTypeFlags.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Types {
    [Flags]
    public enum DataTypeFlags : uint {
        None = 0u,
        Nullable = 1u << 1,

        #region Category
        Primitive = 1u << 2,
        Complex = 1u << 3,
        Collection = 1u << 4,
        #endregion

        #region PrimitiveType
        Guid = 1u << 5,
        Bool = 1u << 6,
        Char = 1u << 7,
        Enum = 1u << 8,

        SByte = 1u << 9,
        Byte = 1u << 10,
        Short = 1u << 11,
        UShort = 1u << 12,
        Int = 1u << 13,
        UInt = 1u << 14,
        Long = 1u << 15,
        ULong = 1u << 16,
        Float = 1u << 17,
        Double = 1u << 18,
        Decimal = 1u << 19,

        DateTime = 1u << 20,
        DateTimeOffset = 1u << 21,
        TimeSpan = 1u << 22,

        String = 1u << 23,
        Object = 1u << 24,
        #endregion

        #region Mask
        NumericMask = Byte | Short | UShort | Int | UInt | Long | ULong,
        PrimitiveMask = Guid | Bool | Enum | TextMask | NumericMask | TimeMask | Object,
        TimeMask = DateTime | TimeSpan,
        TextMask = String | Char,
        CategoryMask = Primitive | Complex | Collection,
        #endregion
    }
}
