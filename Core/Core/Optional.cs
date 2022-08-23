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
    /// <summary>
    /// A class to define optional value types
    /// </summary>
    public struct Optional<TValue> : IOptional<TValue>{

        /// <summary>
        /// A static generic value representing the undefined state
        /// </summary>
        public static Optional<TValue> Undefined => new();
        /// <inheritdoc />
        public bool IsDefined { get; }

        /// <inheritdoc />
        public bool IsUndefined => IsDefined == false;
        /// <inheritdoc />
        public bool IsDefault => IsDefined && Equals(Value, default(TValue));
        /// <inheritdoc />
        public bool IsUndefinedOrDefault => (IsDefined == false) || Equals(Value, default(TValue));
        object IOptional.Value => Value;


        object IOptional.GetValue() {
            return GetValue();
        }

        object IOptional.GetValueOrDefault(object defaultValue) {
            return GetValueOrDefault((TValue)defaultValue);
        }

        bool IOptional.TryGetValue(out object value) {
            if(TryGetValue(out var tvalue)) {
                value = tvalue;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        public TValue Value { get; }

        /// <summary>
        /// Creates a new instance of <see cref="Optional{TValue}"/>
        /// </summary>
        /// <param name="value"></param>
        public Optional(TValue value) {
            IsDefined = true;
            Value = value;
        }

        /// <inheritdoc />
        public TValue GetValue() {
            return IsDefined ? Value : throw new Exception("A value is required, but not defined");
        }

        /// <inheritdoc />
        public TValue GetValueOrDefault(TValue defaultValue = default) {
            return IsDefined ? Value : defaultValue;
        }

        /// <inheritdoc />
        public bool TryGetValue(out TValue defaultValue) {
            defaultValue = Value;
            return IsDefined;
        }

        /// <inheritdoc />
        public void ThrowIfUndefined() {
            throw new Exception("A value is required, but not defined");
        }

        /// <inheritdoc />
        public void ThrowIfUndefinedOrDefault() {
            throw new Exception("A value is required, but not defined or equals the default value");
        }

        /// <summary>
        /// Converts a value to a wrapped representation
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Optional<TValue>(TValue value) {
            return new Optional<TValue>(value);
        }


        /// <summary>
        /// Explicitly converts a wrapped representation to its inner value<br></br>
        /// If the value is undefined an exception is thrown
        /// </summary>
        /// <exception cref="InvalidOperationException">The value is undefined</exception>
        public static explicit operator TValue(Optional<TValue> value) {
            if(value.IsDefined == false)
                throw new InvalidOperationException("Value is not defined");

            return value.Value;
        }

        /// <summary>
        /// Checks if the value is equal to another optional value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Optional<TValue> other) {
            return (IsDefined == other.IsDefined) && (IsDefined == false || EqualityComparer<TValue>.Default.Equals(Value, other.Value));
        }

        /// <summary>
        /// Checks if the inner value is equal to another optional value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
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
            return obj switch {
                null => Equals(Value, null),
                Optional<TValue> optional => Equals(optional),
                TValue value => Equals(value),
                _ => false
            };
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode() {
            return IsDefined && Value is not null ? Value.GetHashCode() : 0;
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