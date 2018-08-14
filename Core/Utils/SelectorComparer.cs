// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SelectorComparer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Utils {
    public class SelectorComparer<T, TKey> : IComparer<T> where TKey : IComparable {
        private readonly Func<T, TKey> _comparableSelector;

        public SelectorComparer(Func<T, TKey> comparableSelector) {
            _comparableSelector = comparableSelector;
        }

        public int Compare(T x, T y) {
            return _comparableSelector.Invoke(x).CompareTo(_comparableSelector.Invoke(y));
        }
    }
}
