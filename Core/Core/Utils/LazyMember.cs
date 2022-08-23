// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LazyMember.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Utils {
    /// <summary>
    /// A property created on the fly
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class LazyMember<TValue> {
        private readonly object _lock = new();
        private readonly Func<TValue> _valueInitializer;
        private TValue _value;

        /// <summary>
        /// Checks if the value has been created already
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Get or create a value
        /// </summary>
        public TValue Value {
            get {
                if(HasValue) //Atomic read
                    return _value;
                EnsureValue();
                return _value;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="LazyMember{TValue}"/>
        /// </summary>
        /// <param name="valueInitializer"></param>
        public LazyMember(Func<TValue> valueInitializer) {
            _valueInitializer = valueInitializer ?? throw new ArgumentNullException(nameof(valueInitializer));
        }

        /// <summary>
        /// Get or create the value manually
        /// </summary>
        public void EnsureValue() {
            lock(_lock) {
                if(HasValue) //Recheck because of possible racing condition
                    return;
                _value = _valueInitializer.Invoke();
                HasValue = true;
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="LazyMember{TValue}"/> using an initializer function
        /// </summary>
        /// <param name="valueInitializer"></param>
        public static implicit operator LazyMember<TValue>(Func<TValue> valueInitializer) {
            return new LazyMember<TValue>(valueInitializer);
        }
    }
}