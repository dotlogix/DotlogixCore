// ==================================================
// Copyright 2016(C) , DotLogix
// File:  Enum.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Enumeration {
    public abstract class Enum<TEnum, TEnumMember> : IEnum<TEnum, TEnumMember>
        where TEnum : class, IEnum<TEnum, TEnumMember>
        where TEnumMember : class, IEnumMember<TEnum, TEnumMember> {
        private readonly Dictionary<string, TEnumMember> _nameMapping = new Dictionary<string, TEnumMember>();
        private readonly Dictionary<int, TEnumMember> _valueMapping = new Dictionary<int, TEnumMember>();

        protected Enum() {
            var realtype = GetType();
            var enumtype = typeof(TEnum);

            if(realtype.IsSealed == false)
                throw new EnumException("Only sealed enum types can be instantiated");
            if(realtype != enumtype)
                throw new EnumException($"{nameof(TEnum)} has to be the same type as the instantiated type");

            Name = realtype.Name;
            SupportsFlags = realtype.IsDefined(typeof(SupportsFlagsAttribute), true);
        }

        protected TEnumMember Register(TEnumMember member) {
            _valueMapping.Add(member.Value, member);
            _nameMapping.Add(member.Name, member);
            return member;
        }

        public override string ToString() {
            return Name;
        }

        #region IEnum<TEnum, TEnumMember>
        public IEnumerable<TEnumMember> Members => _valueMapping.Values.ToList();

        public TEnumMember this[int value] => GetByValue(value);

        public TEnumMember this[string name] => GetByName(name);

        public TEnumMember GetByValue(int value) {
            return _valueMapping.TryGetValue(value, out var member) ? member : null;
        }

        public TEnumMember GetByName(string name) {
            return _nameMapping.TryGetValue(name, out var member) ? member : null;
        }
        #endregion

        #region IEnum
        public string Name { get; }
        public bool SupportsFlags { get; }
        IEnumerable<IEnumMember> IEnum.Members => Members;
        IEnumMember IEnum.this[int value] => GetByValue(value);
        IEnumMember IEnum.this[string name] => GetByName(name);
        IEnumMember IEnum.GetByValue(int value) => GetByValue(value);
        IEnumMember IEnum.GetByName(string name) => GetByName(name);

        public bool IsDefined(int value) {
            return _valueMapping.ContainsKey(value);
        }

        public bool IsDefined(string name) {
            return _nameMapping.ContainsKey(name);
        }
        #endregion
    }
}
