// ==================================================
// Copyright 2019(C) , DotLogix
// File:  KeyedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  07.02.2019
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Collections {
    /// <summary>
    /// A collection indexed by a unique key
    /// </summary>
    /// <typeparam name="TKey">They key</typeparam>
    /// <typeparam name="TValue">The value</typeparam>
    public class KeyedCollection<TKey, TValue> : ICollection<TValue>, IReadOnlyCollection<TValue> {
        /// <summary>
        /// The internal dictionary
        /// </summary>
        protected ConcurrentDictionary<TKey, TValue> InnerDictionary { get; }
        /// <summary>
        /// The selector function
        /// </summary>
        protected Func<TValue, TKey> KeySelector { get; }

        /// <summary>
        /// Get a value by its key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        public TValue this[TKey key] => InnerDictionary[key];

        /// <summary>
        /// Creates an instance of <see cref="KeyedCollection{TKey,TValue}"/>
        /// </summary>
        /// <param name="keySelector">The key selector</param>
        public KeyedCollection(Func<TValue, TKey> keySelector) {
            KeySelector = keySelector;
            InnerDictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        /// <summary>
        /// Creates an instance of <see cref="KeyedCollection{TKey,TValue}"/>
        /// </summary>
        /// <param name="keySelector">The key selector</param>
        /// <param name="values">The initial values</param>
        public KeyedCollection(Func<TValue, TKey> keySelector, IEnumerable<TValue> values) {
            KeySelector = keySelector;
            InnerDictionary = new ConcurrentDictionary<TKey, TValue>(values.ToDictionary(keySelector));
        }

        /// <summary>
        /// Creates an instance of <see cref="KeyedCollection{TKey,TValue}"/>
        /// </summary>
        /// <param name="keySelector">The key selector</param>
        /// <param name="equalityComparer">The equality comparer</param>
        public KeyedCollection(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> equalityComparer) {
            KeySelector = keySelector;
            InnerDictionary = new ConcurrentDictionary<TKey, TValue>(equalityComparer);
        }

        /// <summary>
        /// Creates an instance of <see cref="KeyedCollection{TKey,TValue}"/>
        /// </summary>
        /// <param name="keySelector">The key selector</param>
        /// <param name="equalityComparer">The equality comparer</param>
        /// <param name="values">The initial values</param>
        public KeyedCollection(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> equalityComparer, IEnumerable<TValue> values) {
            KeySelector = keySelector;
            InnerDictionary = new ConcurrentDictionary<TKey, TValue>(values.ToDictionary(keySelector, equalityComparer));
        }


        /// <inheritdoc />
        public void CopyTo(TValue[] array, int arrayIndex) {
            InnerDictionary.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the amount of items in the collection
        /// </summary>
        public int Count => InnerDictionary.Count;
        /// <summary>
        /// The keys of the collection
        /// </summary>
        public IEnumerable<TKey> Keys => InnerDictionary.Keys.ToList();

        /// <summary>
        /// The key value paires of the collection
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs => InnerDictionary.ToList();

        /// <inheritdoc />
        bool ICollection<TValue>.IsReadOnly => false;

        /// <summary>
        /// Removes all values from the collection
        /// </summary>
        public void Clear() {
            InnerDictionary.Clear();
        }

        /// <summary>
        /// Checks if the collection contains a value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(TValue value) {
            return InnerDictionary.ContainsKey(KeySelector.Invoke(value));
        }

        /// <summary>
        /// Add an item to the collection
        /// </summary>
        /// <param name="value"></param>
        void ICollection<TValue>.Add(TValue value) {
            if(TryAdd(value) == false)
                throw new Exception("A value with this key already exists");
        }

        /// <summary>
        /// Remove an item from the collection
        /// </summary>
        /// <param name="value"></param>
        bool ICollection<TValue>.Remove(TValue value) {
            return TryRemove(value);
        }

        /// <inheritdoc />
        public IEnumerator<TValue> GetEnumerator() {
            return InnerDictionary.Values.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Tries to add a value to the collection
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(TValue value) {
            return InnerDictionary.TryAdd(KeySelector.Invoke(value), value);
        }

        /// <summary>
        /// Get the existing item or adds a new one matched by their keys
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TValue GetOrAdd(TValue value) {
            return InnerDictionary.GetOrAdd(KeySelector.Invoke(value), value);
        }

        /// <summary>
        /// Add or update a value to the collection
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void AddOrUpdate(TValue value) {
            InnerDictionary[KeySelector.Invoke(value)] = value;
        }

        /// <summary>
        /// Tries to remove a value from the collection
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryRemove(TValue value) {
            return InnerDictionary.TryRemove(KeySelector.Invoke(value), out _);
        }

        /// <summary>
        /// Tries to get a value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(TKey key, out TValue value) {
            return InnerDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Tries to remove the value with the matching key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool TryRemoveKey(TKey key) {
            return InnerDictionary.TryRemove(key, out _);
        }

        /// <summary>
        /// Checks if a key exists in the collection
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key) {
            return InnerDictionary.ContainsKey(key);
        }
    }
}
