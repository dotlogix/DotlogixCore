// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  MergingDictionary.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Collections {
    /// <summary>
    /// A dictionary which merges values together
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MergingDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
        public IDictionary<TKey, TValue> InnerDictionary { get; }
        private readonly Func<TValue, TValue, TValue> _mergeFunc;

        /// <inheritdoc />
        public MergingDictionary(Func<TValue, TValue, TValue> mergeFunc) {
            _mergeFunc = mergeFunc;
        }

        /// <inheritdoc />
        public MergingDictionary(IDictionary<TKey, TValue> dictionary, Func<TValue, TValue, TValue> mergeFunc) {
            _mergeFunc = mergeFunc;
            InnerDictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <inheritdoc />
        public MergingDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer, Func<TValue, TValue, TValue> mergeFunc) {
            _mergeFunc = mergeFunc;
            InnerDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <inheritdoc />
        public MergingDictionary(IEqualityComparer<TKey> comparer, Func<TValue, TValue, TValue> mergeFunc) {
            _mergeFunc = mergeFunc;
            InnerDictionary = new Dictionary<TKey, TValue>(comparer);
        }

        /// <inheritdoc />
        public MergingDictionary(int capacity, Func<TValue, TValue, TValue> mergeFunc) {
            _mergeFunc = mergeFunc;
            InnerDictionary = new Dictionary<TKey, TValue>(capacity);
        }

        /// <inheritdoc />
        public MergingDictionary(int capacity, IEqualityComparer<TKey> comparer, Func<TValue, TValue, TValue> mergeFunc) {
            _mergeFunc = mergeFunc;
            InnerDictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return InnerDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)InnerDictionary).GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item) {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public void Clear() {
            InnerDictionary.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return InnerDictionary.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            InnerDictionary.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return InnerDictionary.Remove(item);
        }

        /// <inheritdoc />
        public int Count => InnerDictionary.Count;

        /// <inheritdoc />
        public bool IsReadOnly => InnerDictionary.IsReadOnly;

        /// <inheritdoc />
        public void Add(TKey key, TValue value) {
            if(InnerDictionary.TryGetValue(key, out var current)) {
                value = _mergeFunc.Invoke(value, current);
            }

            InnerDictionary[key] = value;
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key) {
            return InnerDictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool Remove(TKey key) {
            return InnerDictionary.Remove(key);
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value) {
            return InnerDictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc />
        public TValue this[TKey key] {
            get => InnerDictionary[key];
            set => Add(key, value);
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys => InnerDictionary.Keys;

        /// <inheritdoc />
        public ICollection<TValue> Values => InnerDictionary.Values;
    }

}
