// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Optional.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core {
    public struct Optional<TValue> {
        public static Optional<TValue> Undefined => new Optional<TValue>();
        public bool IsDefined { get; }
        public bool IsDefault => IsDefined && Equals(Value, default(TValue));
        public bool IsUndefinedOrDefault => (IsDefined == false) || Equals(Value, default(TValue));
        public TValue Value { get; }

        public Optional(TValue value) {
            IsDefined = true;
            Value = value;
        }

        public TValue GetValueOrDefault(TValue defaultValue = default) {
            return IsDefined ? Value : defaultValue;
        }

        public bool TryGetValue(out TValue defaultValue) {
            defaultValue = Value;
            return IsDefined;
        }


        public static implicit operator Optional<TValue>(TValue value) {
            return new Optional<TValue>(value);
        }

        public static explicit operator TValue(Optional<TValue> value) {
            if(value.IsDefined == false)
                throw new InvalidOperationException("Value is not defined");

            return value.Value;
        }

        public bool Equals(Optional<TValue> other) {
            return (IsDefined == other.IsDefined) && (IsDefined==false || EqualityComparer<TValue>.Default.Equals(Value, other.Value));
        }

        public bool Equals(TValue other) {
            return IsDefined && EqualityComparer<TValue>.Default.Equals(Value, other);
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        ///     true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value;
        ///     otherwise, false.
        /// </returns>
        public override bool Equals(object obj) {
            switch(obj) {
                case null:
                    return Equals(Value, null);
                case Optional<TValue> optional:
                    return Equals(optional);
                case TValue value:
                    return Equals(value);
            }
            return false;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode() {
            return IsDefined && Value != null ? Value.GetHashCode() : 0;
        }

        /// <summary>
        ///     Returns a value that indicates whether the values of two <see cref="T:DotLogix.Core.Optional`1" /> objects are
        ///     equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(Optional<TValue> left, Optional<TValue> right) {
            return left.Equals(right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:DotLogix.Core.Optional`1" /> objects have different
        ///     values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(Optional<TValue> left, Optional<TValue> right) {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Returns a value that indicates whether the values of two <see cref="T:DotLogix.Core.Optional`1" /> objects are
        ///     equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(Optional<TValue> left, TValue right) {
            return left.Equals(right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:DotLogix.Core.Optional`1" /> objects have different
        ///     values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(Optional<TValue> left, TValue right) {
            return !left.Equals(right);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() {
            if(IsDefined)
                return Value == null ? "null" : Value.ToString();
            return "undefined";
        }
    }
}
