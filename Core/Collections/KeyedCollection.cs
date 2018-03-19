// ==================================================
// Copyright 2018(C) , DotLogix
// File:  KeyedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  05.03.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Collections {
    public class KeyedCollection<TKey, TValue> : ICollection<TValue>, IReadOnlyCollection<TValue> {
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;
        private readonly Func<TValue, TKey> _keySelector;

        public TValue this[TKey key] => _dictionary[key];

        public KeyedCollection(Func<TValue, TKey> keySelector) {
            _keySelector = keySelector;
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        public KeyedCollection(Func<TValue, TKey> keySelector, IEnumerable<TValue> values) {
            _keySelector = keySelector;
            _dictionary = new ConcurrentDictionary<TKey, TValue>(values.ToDictionary(keySelector));
        }

        public KeyedCollection(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> equalityComparer) {
            _keySelector = keySelector;
            _dictionary = new ConcurrentDictionary<TKey, TValue>(equalityComparer);
        }

        public KeyedCollection(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> equalityComparer, IEnumerable<TValue> values) {
            _keySelector = keySelector;
            _dictionary = new ConcurrentDictionary<TKey, TValue>(values.ToDictionary(keySelector,equalityComparer));
        }

        public void CopyTo(TValue[] array, int arrayIndex) {
            _dictionary.Values.CopyTo(array, arrayIndex);
        }

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public void Clear() {
            _dictionary.Clear();
        }

        public bool Contains(TValue value) {
            return _dictionary.ContainsKey(_keySelector.Invoke(value));
        }

        public bool TryAdd(TValue value) {
            return _dictionary.TryAdd(_keySelector.Invoke(value), value);
        }

        public bool TryRemove(TValue value) {
            return _dictionary.TryRemove(_keySelector.Invoke(value), out _);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return _dictionary.TryGetValue(key, out value);
        }

        public bool TryRemoveKey(TKey key) {
            return _dictionary.TryRemove(key, out _);
        }

        public bool ContainsKey(TKey key) {
            return _dictionary.ContainsKey(key);
        }

        void ICollection<TValue>.Add(TValue value)
        {
            if(TryAdd(value) == false)
                throw new Exception("A value with this key already exists");
        }
        bool ICollection<TValue>.Remove(TValue value) {
            return TryRemove(value);
        }

        public IEnumerator<TValue> GetEnumerator() {
            return _dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
