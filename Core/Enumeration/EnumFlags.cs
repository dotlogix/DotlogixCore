// ==================================================
// Copyright 2016(C) , DotLogix
// File:  EnumFlags.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  10.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
using DotLogix.Core.Interfaces;
#endregion

namespace DotLogix.Core.Enumeration {
    public sealed class EnumFlags<TEnum, TEnumMember> : ICloneable<EnumFlags<TEnum, TEnumMember>>
        where TEnum : class, IEnum<TEnum, TEnumMember>
        where TEnumMember : class, IEnumMember<TEnum, TEnumMember> {
        private readonly HashSet<TEnumMember> _flags;
        private string _name;

        public TEnum DefiningEnum { get; }
        public bool IsSingleFlag => _flags.Count == 1;
        public bool IsEmpty => _flags.Count == 0;
        public IEnumerable<TEnumMember> Flags => _flags.ToList();
        public IEnumerable<TEnumMember> OrderedFlags => Flags.OrderBy(e => e.Value).ToList();

        public string Name => _name ?? (_name = string.Join(", ", OrderedFlags.Select(f => f.Name)));
        public int Value { get; private set; }

        public EnumFlags(TEnumMember member) : this(member.DefiningEnum, member) { }

        public EnumFlags(TEnum definingEnum, params TEnumMember[] members) : this(definingEnum, members.ToHashSet()) { }

        public EnumFlags(TEnum definingEnum, IEnumerable<TEnumMember> members) :
            this(definingEnum, members?.ToHashSet()) { }

        private EnumFlags(TEnum definingEnum, HashSet<TEnumMember> members = null) {
            DefiningEnum = definingEnum ?? throw new ArgumentNullException(nameof(definingEnum));
            if(members == null) {
                _flags = new HashSet<TEnumMember>();
                Value = 0;
            } else {
                _flags = members;
                Value = _flags.Aggregate(0, (v, member) => v | member.Value);
            }
        }

        public EnumFlags<TEnum, TEnumMember> Clone() {
            return new EnumFlags<TEnum, TEnumMember>(DefiningEnum, _flags.ToHashSet());
        }

        object ICloneable.Clone() {
            return Clone();
        }

        private bool Equals(EnumFlags<TEnum, TEnumMember> other) {
            return Value == other.Value;
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            var flags = obj as EnumFlags<TEnum, TEnumMember>;
            return (flags != null) && Equals(flags);
        }


        /// <summary>
        ///     Returns a value that indicates whether the values of two
        ///     <see cref="T:DotLogix.Core.Enumeration.EnumFlags`2" /> objects are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(EnumFlags<TEnum, TEnumMember> left, EnumFlags<TEnum, TEnumMember> right) {
            return Equals(left, right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:DotLogix.Core.Enumeration.EnumFlags`2" /> objects have
        ///     different values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(EnumFlags<TEnum, TEnumMember> left, EnumFlags<TEnum, TEnumMember> right) {
            return !Equals(left, right);
        }

        public bool AddFlag(TEnumMember member) {
            if(HasFlag(member))
                return false;
            _flags.Add(member);
            Value |= member.Value;
            _name = null;
            return true;
        }

        public bool RemoveFlag(TEnumMember member) {
            if(HasFlag(member) == false)
                return false;
            _flags.Remove(member);
            Value &= ~member.Value;
            _name = null;
            return true;
        }

        public bool HasFlag(TEnumMember member) {
            return (Value & member.Value) != 0;
        }

        public bool AddFlags(EnumFlags<TEnum, TEnumMember> flags) {
            if(HasAllFlags(flags))
                return false;

            _flags.UnionWith(flags._flags);
            Value |= flags.Value;
            _name = null;
            return true;
        }

        public bool RemoveFlags(EnumFlags<TEnum, TEnumMember> flags) {
            if(HasAnyFlags(flags) == false)
                return false;

            _flags.ExceptWith(flags._flags);
            Value &= ~flags.Value;
            _name = null;
            return true;
        }

        public bool HasAllFlags(EnumFlags<TEnum, TEnumMember> flags) {
            return flags.IsEmpty || ((Value & flags.Value) == flags.Value);
        }

        public bool HasAnyFlags(EnumFlags<TEnum, TEnumMember> flags) {
            return (Value & flags.Value) != 0;
        }

        public bool AddFlags(IEnumerable<TEnumMember> members) {
            return members.Aggregate(false, (current, member) => current | AddFlag(member));
        }

        public bool RemoveFlags(IEnumerable<TEnumMember> members) {
            return members.Aggregate(false, (current, member) => current | RemoveFlag(member));
        }

        public bool HasAllFlags(IEnumerable<TEnumMember> members) {
            return members.All(HasFlag);
        }

        public bool HasAnyFlags(IEnumerable<TEnumMember> members) {
            return members.Any(HasFlag);
        }

        public bool AddFlags(params TEnumMember[] members) {
            return AddFlags(members.AsEnumerable());
        }

        public bool RemoveFlags(params TEnumMember[] members) {
            return RemoveFlags(members.AsEnumerable());
        }

        public bool HasAllFlags(params TEnumMember[] members) {
            return HasAllFlags(members.AsEnumerable());
        }

        public bool HasAnyFlags(params TEnumMember[] members) {
            return HasAnyFlags(members.AsEnumerable());
        }

        public static EnumFlags<TEnum, TEnumMember> And(EnumFlags<TEnum, TEnumMember> a,
                                                        EnumFlags<TEnum, TEnumMember> b) {
            if(a.DefiningEnum != b.DefiningEnum)
                throw new InvalidOperationException();

            var hashSet = a._flags.ToHashSet();
            hashSet.IntersectWith(b._flags);
            return new EnumFlags<TEnum, TEnumMember>(a.DefiningEnum, hashSet);
        }

        public static EnumFlags<TEnum, TEnumMember> Or(EnumFlags<TEnum, TEnumMember> a,
                                                       EnumFlags<TEnum, TEnumMember> b) {
            if(a.DefiningEnum != b.DefiningEnum)
                throw new InvalidOperationException();

            var flags = a.Clone();
            flags.AddFlags(b);
            return flags;
        }

        public static EnumFlags<TEnum, TEnumMember> Not(EnumFlags<TEnum, TEnumMember> a) {
            var hashSet = new HashSet<TEnumMember>();
            foreach(var member in a.DefiningEnum.Members) {
                if(a.HasFlag(member) == false)
                    hashSet.Add(member);
            }
            return new EnumFlags<TEnum, TEnumMember>(a.DefiningEnum, hashSet);
        }

        public static EnumFlags<TEnum, TEnumMember> XOr(EnumFlags<TEnum, TEnumMember> a,
                                                        EnumFlags<TEnum, TEnumMember> b) {
            if(a.DefiningEnum != b.DefiningEnum)
                throw new InvalidOperationException();
            var hashset = a._flags.ToHashSet();
            hashset.SymmetricExceptWith(b._flags);
            return new EnumFlags<TEnum, TEnumMember>(a.DefiningEnum, hashset);
        }

        public static EnumFlags<TEnum, TEnumMember> operator |(EnumFlags<TEnum, TEnumMember> a,
                                                               EnumFlags<TEnum, TEnumMember> b) {
            return Or(a, b);
        }

        public static EnumFlags<TEnum, TEnumMember> operator &(EnumFlags<TEnum, TEnumMember> a,
                                                               EnumFlags<TEnum, TEnumMember> b) {
            return And(a, b);
        }

        public static EnumFlags<TEnum, TEnumMember> operator ^(EnumFlags<TEnum, TEnumMember> a,
                                                               EnumFlags<TEnum, TEnumMember> b) {
            return XOr(a, b);
        }

        public static EnumFlags<TEnum, TEnumMember> operator ~(EnumFlags<TEnum, TEnumMember> a) {
            return Not(a);
        }

        public static implicit operator EnumFlags<TEnum, TEnumMember>(TEnumMember member) {
            return new EnumFlags<TEnum, TEnumMember>(member);
        }

        public static explicit operator TEnumMember(EnumFlags<TEnum, TEnumMember> flags) {
            if(flags.IsSingleFlag)
                return flags.DefiningEnum.GetByValue(flags.Value);
            throw new InvalidCastException();
        }
    }
}
