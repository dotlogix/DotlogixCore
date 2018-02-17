// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LazyMember.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Utils {
    public class LazyMember<TValue> {
        private readonly object _lock = new object();
        private readonly Func<TValue> _valueInitializer;
        private TValue _value;

        public bool HasValue { get; private set; }

        public TValue Value {
            get {
                if(HasValue) //Atomic read
                    return _value;
                EnsureValue();
                return _value;
            }
        }

        public LazyMember(Func<TValue> valueInitializer) {
            _valueInitializer = valueInitializer ?? throw new ArgumentNullException(nameof(valueInitializer));
        }

        public void EnsureValue() {
            lock(_lock) {
                if(HasValue) //Recheck because of possible racing condition
                    return;
                _value = _valueInitializer.Invoke();
                HasValue = true;
            }
        }

        public static implicit operator LazyMember<TValue>(Func<TValue> valueInitializer) {
            return new LazyMember<TValue>(valueInitializer);
        }
    }
}
