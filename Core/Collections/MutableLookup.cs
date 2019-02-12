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
#endregion

namespace DotLogix.Core.Collections {
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
        private TCollection this[TKey key] {
            get { return _innerDictionary[key]; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lookup`3" /> class that is empty, has the specified initial
        ///     capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:Lookup`3" /> can contain.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="capacity" /> is less than 0.
        /// </exception>
        public MutableLookup(int capacity, Func<TCollection> instantiateFunc) {
            _instantiateFunc = instantiateFunc;
            _innerDictionary = new Dictionary<TKey, TCollection>(capacity);
        }
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lookup`3" /> class that is empty
        /// </summary>
        public MutableLookup(Func<TCollection> instantiateFunc) {
            _instantiateFunc = instantiateFunc;
            _innerDictionary = new Dictionary<TKey, TCollection>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lookup`3" /> class that is empty, has the default initial
        ///     capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="comparer">
        ///     The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when
        ///     comparing keys, or <see langword="null" /> to use the default
        ///     <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.
        /// </param>
        public MutableLookup(IEqualityComparer<TKey> comparer, Func<TCollection> instantiateFunc) {
            _instantiateFunc = instantiateFunc;
            _innerDictionary = new Dictionary<TKey, TCollection>(comparer);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lookup`3" /> class that is empty, has the specified initial
        ///     capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:Lookup`3" /> can contain.</param>
        /// <param name="comparer">
        ///     The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when
        ///     comparing keys, or <see langword="null" /> to use the default
        ///     <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="capacity" /> is less than 0.
        /// </exception>
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

        /// <summary>Determines whether a specified key exists in the <see cref="T:System.Linq.ILookup`2"></see>.</summary>
        /// <param name="key">The key to search for in the <see cref="T:System.Linq.ILookup`2"></see>.</param>
        /// <returns>
        ///     true if <paramref name="key">key</paramref> is in the <see cref="T:System.Linq.ILookup`2"></see>; otherwise,
        ///     false.
        /// </returns>
        public bool Contains(TKey key) {
            return _innerDictionary.ContainsKey(key);
        }

        /// <summary>Gets the number of key/value collection pairs in the <see cref="T:System.Linq.ILookup`2"></see>.</summary>
        /// <returns>The number of key/value collection pairs in the <see cref="T:System.Linq.ILookup`2"></see>.</returns>
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
        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] {
            get { return _innerDictionary[key]; }
        }

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
            if(_innerDictionary.TryGetValue(key, out var collection) == false) {
                collection = _instantiateFunc.Invoke();
                _innerDictionary.Add(key, collection);
            }

            collection.Add(value);
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

    public class MutableLookup<TKey, TValue> : MutableLookup<TKey, TValue, List<TValue>> {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lookup`3" /> class that is empty, has the specified initial
        ///     capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public MutableLookup() : base(CreateCollection) { }
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lookup`3" /> class that is empty, has the specified initial
        ///     capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:Lookup`3" /> can contain.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="capacity" /> is less than 0.
        /// </exception>
        public MutableLookup(int capacity) : base(capacity, CreateCollection) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lookup`3" /> class that is empty, has the default initial
        ///     capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="comparer">
        ///     The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when
        ///     comparing keys, or <see langword="null" /> to use the default
        ///     <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.
        /// </param>
        public MutableLookup(IEqualityComparer<TKey> comparer) : base(comparer, CreateCollection) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Lookup`3" /> class that is empty, has the specified initial
        ///     capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:Lookup`3" /> can contain.</param>
        /// <param name="comparer">
        ///     The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when
        ///     comparing keys, or <see langword="null" /> to use the default
        ///     <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="capacity" /> is less than 0.
        /// </exception>
        public MutableLookup(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer, CreateCollection) { }

        private static List<TValue> CreateCollection() {
            return new List<TValue>();
        }
    }
}
