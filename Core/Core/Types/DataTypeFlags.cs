// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DataTypeFlags.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Types; 

/// <summary>
///     Data type flags
/// </summary>
[Flags]
public enum DataTypeFlags : uint {
    /// <summary>
    ///     None
    /// </summary>
    None = 0u,

    /// <summary>
    ///     Nullable
    /// </summary>
    Nullable = 1u << 1,

    #region Category
    /// <summary>
    ///     Primitive value
    /// </summary>
    Primitive = 1u << 2,

    /// <summary>
    ///     Complex value
    /// </summary>
    Complex = 1u << 3,

    /// <summary>
    ///     Collection type
    /// </summary>
    Collection = 1u << 4,
    #endregion

    #region PrimitiveType
    /// <summary>
    ///     Guid
    /// </summary>
    Guid = 1u << 5,

    /// <summary>
    ///     Bool
    /// </summary>
    Bool = 1u << 6,

    /// <summary>
    ///     Char
    /// </summary>
    Char = 1u << 7,

    /// <summary>
    ///     Enum
    /// </summary>
    Enum = 1u << 8,

    /// <summary>
    ///     SByte
    /// </summary>
    SByte = 1u << 9,

    /// <summary>
    ///     Byte
    /// </summary>
    Byte = 1u << 10,

    /// <summary>
    ///     Short
    /// </summary>
    Short = 1u << 11,

    /// <summary>
    ///     UShort
    /// </summary>
    UShort = 1u << 12,

    /// <summary>
    ///     Int
    /// </summary>
    Int = 1u << 13,

    /// <summary>
    ///     UInt
    /// </summary>
    UInt = 1u << 14,

    /// <summary>
    ///     Long
    /// </summary>
    Long = 1u << 15,

    /// <summary>
    ///     ULong
    /// </summary>
    ULong = 1u << 16,

    /// <summary>
    ///     Float
    /// </summary>
    Float = 1u << 17,

    /// <summary>
    ///     Double
    /// </summary>
    Double = 1u << 18,

    /// <summary>
    ///     Decimal
    /// </summary>
    Decimal = 1u << 19,

    /// <summary>
    ///     DateTime
    /// </summary>
    DateTime = 1u << 20,

    /// <summary>
    ///     DateTimeOffset
    /// </summary>
    DateTimeOffset = 1u << 21,

    /// <summary>
    ///     TimeSpan
    /// </summary>
    TimeSpan = 1u << 22,

    /// <summary>
    ///     String
    /// </summary>
    String = 1u << 23,

    /// <summary>
    ///     Object
    /// </summary>
    Object = 1u << 24,
    #endregion

    #region Mask
    /// <summary>
    ///     NumericMask
    /// </summary>
    NumericMask = ULong | Long | UInt | Int | UShort | Short | Byte | SByte | Float | Double | Decimal,

    /// <summary>
    ///     PrimitiveMask
    /// </summary>
    PrimitiveMask = Guid | Bool | Enum | TextMask | NumericMask | TimeMask | Object,

    /// <summary>
    ///     TimeMask
    /// </summary>
    TimeMask = DateTime | TimeSpan,

    /// <summary>
    ///     TextMask
    /// </summary>
    TextMask = String | Char,

    /// <summary>
    ///     CategoryMask
    /// </summary>
    CategoryMask = Primitive | Complex | Collection,
    #endregion
}