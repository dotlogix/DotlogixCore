// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEnumMember.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Enumeration {
    public interface IEnumMember {
        IEnum DefiningEnum { get; }
        string Name { get; }
        int Value { get; }
    }

    public interface IEnumMember<out TEnum, TEnumMember> : IEnumMember
        where TEnum : class, IEnum<TEnum, TEnumMember>
        where TEnumMember : class, IEnumMember<TEnum, TEnumMember> {
        new TEnum DefiningEnum { get; }
    }
}
