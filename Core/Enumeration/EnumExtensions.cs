// ==================================================
// Copyright 2016(C) , DotLogix
// File:  EnumExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
#endregion

namespace DotLogix.Core.Enumeration {
    public static class EnumExtensions {
        public static EnumFlags<TEnum, TEnumMember> ToFlag<TEnum, TEnumMember>(this TEnumMember member)
            where TEnum : class, IEnum<TEnum, TEnumMember>
            where TEnumMember : class, IEnumMember<TEnum, TEnumMember> {
            return new EnumFlags<TEnum, TEnumMember>(member.DefiningEnum, member);
        }
    }
}
