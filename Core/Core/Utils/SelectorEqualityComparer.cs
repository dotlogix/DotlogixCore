// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SelectorEqualityComparer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Utils {
    /// <inheritdoc />
    public class SelectorEqualityComparer<T, TKey> : IEqualityComparer<T> {
        private readonly Func<T, TKey> _comparableSelector;

        /// <inheritdoc />
        public SelectorEqualityComparer(Func<T, TKey> comparableSelector) {
            _comparableSelector = comparableSelector;
        }

        /// <inheritdoc />
        public bool Equals(T x, T y) {
            return Equals(_comparableSelector.Invoke(x), _comparableSelector.Invoke(y));
        }

        /// <inheritdoc />
        public int GetHashCode(T x) {
            return _comparableSelector.Invoke(x).GetHashCode();
        }
    }
}