// ==================================================
// Copyright 2019(C) , DotLogix
// File:  Lookup.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  07.02.2019
// LastEdited:  07.02.2019
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Collections {
    /// <summary>
    /// A mutable lookup collection
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TCollection"></typeparam>
    public class MutableLookup<TKey, TValue, TCollection> : ILookup<TKey, TValue> where TCollection : ICollection<TValue> {
        private readonly Dictionary<TKey, TCollection> _innerDictionary;
        private readonly Func<TCollection> _instantiateFunc;

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that
        ///     implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public ICollection<TKey> Keys => _innerDictionary.Keys;

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that
        ///     implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public ICollection<TValue> Values => _innerDictionary.Values.SelectMany(v => v).ToList();

        /// <summary>
        ///     Gets the <see cref="T:System.Collections.Generic.IEnumerable`1"></see> sequence of values indexed by a
        ///     specified key.
        /// </summary>
        /// <param name="key">The key of the desired sequence of values.</param>
        /// <returns>
        ///     The <see cref="T:System.Collections.Generic.IEnumerable`1"></see> sequence of values indexed by the specified
        ///     key.
        /// </returns>
        public TCollection this[TKey key] => _innerDictionary[key];

        /// <summary>
        /// Creates a new instance of <see cref="MutableLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        public MutableLookup(int capacity, Func<TCollection> instantiateFunc) {
            _instantiateFunc = instantiateFunc;
            _innerDictionary = new Dictionary<TKey, TCollection>(capacity);
        }
        /// <summary>
        /// Creates a new instance of <see cref="MutableLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        public MutableLookup(Func<TCollection> instantiateFunc) {
            _instantiateFunc = instantiateFunc;
            _innerDictionary = new Dictionary<TKey, TCollection>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="MutableLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        public MutableLookup(IEqualityComparer<TKey> comparer, Func<TCollection> instantiateFunc) {
            _instantiateFunc = instantiateFunc;
            _innerDictionary = new Dictionary<TKey, TCollection>(comparer);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MutableLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        public MutableLookup(int capacity, IEqualityComparer<TKey> comparer, Func<TCollection> instantiateFunc) {
            _instantiateFunc = instantiateFunc;
            _innerDictionary = new Dictionary<TKey, TCollection>(capacity, comparer);
        }


        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator() {
            return _innerDictionary.Select(kv => (IGrouping<TKey, TValue>)new LookupGrouping<TKey, TValue>(kv.Key, kv.Value)).GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the
        ///     collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>Determines whether a specified key exists in the <see cref="MutableLookup{TKey,TValue,TCollection}"></see>.</summary>
        /// <param name="key">The key to search for in the <see cref="MutableLookup{TKey,TValue,TCollection}"></see>.</param>
        /// <returns>
        ///     true if <paramref name="key">key</paramref> is in the <see cref="MutableLookup{TKey,TValue,TCollection}"></see>; otherwise,
        ///     false.
        /// </returns>
        public bool Contains(TKey key) {
            return _innerDictionary.ContainsKey(key);
        }

        /// <summary>Gets the number of key/value collection pairs in the <see cref="MutableLookup{TKey,TValue,TCollection}"></see>.</summary>
        /// <returns>The number of key/value collection pairs in the <see cref="MutableLookup{TKey,TValue,TCollection}"></see>.</returns>
        public int Count => _innerDictionary.Values.Sum(v => v.Count);

        /// <summary>
        ///     Gets the <see cref="T:System.Collections.Generic.IEnumerable`1"></see> sequence of values indexed by a
        ///     specified key.
        /// </summary>
        /// <param name="key">The key of the desired sequence of values.</param>
        /// <returns>
        ///     The <see cref="T:System.Collections.Generic.IEnumerable`1"></see> sequence of values indexed by the specified
        ///     key.
        /// </returns>
        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] => _innerDictionary[key];

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1"></see> is
        ///     read-only.
        /// </exception>
        public void Clear() {
            _innerDictionary.Clear();
        }

        /// <summary>
        ///     Adds an element with the provided key and value to the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///     An element with the same key already exists in the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is
        ///     read-only.
        /// </exception>
        public void Add(TKey key, TValue value) {
            if (_innerDictionary.TryGetValue(key, out var collection) == false) {
                collection = _instantiateFunc.Invoke();
                _innerDictionary.Add(key, collection);
            }

            collection.Add(value);
        }

        /// <summary>
        ///     Adds multiple elements with the provided key and value to the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="values">The object to use as the values of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///     An element with the same key already exists in the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is
        ///     read-only.
        /// </exception>
        public void Add(TKey key, IEnumerable<TValue> values) {
            if (_innerDictionary.TryGetValue(key, out var collection) == false) {
                collection = _instantiateFunc.Invoke();
                _innerDictionary.Add(key, collection);
            }

            collection.AddRange(values);
        }

        /// <summary>
        ///     Removes the element with the specified key from the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        ///     true if the element is successfully removed; otherwise, false.  This method also returns false if
        ///     <paramref name="key">key</paramref> was not found in the original
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is
        ///     read-only.
        /// </exception>
        public bool Remove(TKey key) {
            return _innerDictionary.Remove(key);
        }

        /// <summary>
        ///     Removes the element with the specified key from the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="value">The key of the element to remove.</param>
        /// <returns>
        ///     true if the element is successfully removed; otherwise, false.  This method also returns false if
        ///     <paramref name="key">key</paramref> was not found in the original
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is
        ///     read-only.
        /// </exception>
        public bool Remove(TKey key, TValue value) {
            return _innerDictionary.TryGetValue(key, out var collection) && collection.Remove(value);
        }



        /// <summary>
        ///     Removes the element with the specified key from the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="values">The values of the element to remove.</param>
        /// <returns>
        ///     true if the element is successfully removed; otherwise, false.  This method also returns false if
        ///     <paramref name="key">key</paramref> was not found in the original
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is
        ///     read-only.
        /// </exception>
        public void Remove(TKey key, IEnumerable<TValue> values) {
            if(_innerDictionary.TryGetValue(key, out var collection)) {
                collection.RemoveRange(values.AsCollection());
            }
        }

        /// <summary>Gets the value associated with the specified key.</summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="values">
        ///     When this method returns, the value associated with the specified key, if the key is found;
        ///     otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an
        ///     element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key">key</paramref> is null.</exception>
        public bool TryGetValue(TKey key, out TCollection values) {
            return _innerDictionary.TryGetValue(key, out values);
        }
    }

    /// <summary>
    /// A mutable lookup collection
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MutableLookup<TKey, TValue> : MutableLookup<TKey, TValue, List<TValue>> {
        /// <summary>
        /// Creates a new instance of <see cref="MutableLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        public MutableLookup() : base(CreateCollection) { }
        /// <summary>
        /// Creates a new instance of <see cref="MutableLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        public MutableLookup(int capacity) : base(capacity, CreateCollection) { }

        /// <summary>
        /// Creates a new instance of <see cref="MutableLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        public MutableLookup(IEqualityComparer<TKey> comparer) : base(comparer, CreateCollection) { }

        /// <summary>
        /// Creates a new instance of <see cref="MutableLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        public MutableLookup(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer, CreateCollection) { }

        private static List<TValue> CreateCollection() {
            return new List<TValue>();
        }
    }
}
