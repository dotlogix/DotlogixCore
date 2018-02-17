// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEnum.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Enumeration {
    public interface IEnum {
        string Name { get; }
        bool SupportsFlags { get; }
        IEnumerable<IEnumMember> Members { get; }

        IEnumMember this[int value] { get; }
        IEnumMember this[string name] { get; }

        IEnumMember GetByValue(int value);
        IEnumMember GetByName(string name);

        bool IsDefined(int value);
        bool IsDefined(string name);
    }

    public interface IEnum<TEnum, out TEnumMember> : IEnum
        where TEnum : class, IEnum<TEnum, TEnumMember>
        where TEnumMember : class, IEnumMember<TEnum, TEnumMember> {
        new IEnumerable<TEnumMember> Members { get; }

        new TEnumMember this[int value] { get; }
        new TEnumMember this[string name] { get; }

        new TEnumMember GetByValue(int value);
        new TEnumMember GetByName(string name);
    }
}
