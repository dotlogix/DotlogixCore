// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EnumMember.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Enumeration {
    public abstract class EnumMember<TEnum, TEnumMember> : IEnumMember<TEnum, TEnumMember>
        where TEnum : class, IEnum<TEnum, TEnumMember>
        where TEnumMember : class, IEnumMember<TEnum, TEnumMember> {
        protected EnumMember(TEnum definingColorEnum, string name, int value) {
            var realtype = GetType();
            var enumtype = typeof(TEnumMember);

            if(realtype.IsSealed == false)
                throw new EnumException("Only sealed enum member types can be instantiated");
            if(realtype != enumtype)
                throw new EnumException($"{nameof(TEnumMember)} has to be the same type as the instantiated type");

            DefiningEnum = definingColorEnum ?? throw new ArgumentNullException(nameof(definingColorEnum));
            Name = name ?? throw new ArgumentNullException(nameof(name));

            if(DefiningEnum.SupportsFlags && (value.IsPowerOfTwo() == false))
                throw new EnumException("The Value of an enum member in a flags enum must be a power of two");
            Value = value;
        }

        IEnum IEnumMember.DefiningEnum => DefiningEnum;
        public TEnum DefiningEnum { get; }
        public string Name { get; }
        public int Value { get; }

        public override string ToString() {
            return $"{DefiningEnum.Name}.{Name}";
        }
    }
}
