// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EnumExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
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
