// ==================================================
// Copyright 2019(C) , DotLogix
// File:  KeyedLookup.cs
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
using DotLogix.Core.Extensions;

#endregion

namespace DotLogix.Core.Collections {

    /// <summary>
    /// A mutable lookup collection
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyedLookup<TKey, TValue> : KeyedLookup<TKey, TValue, List<TValue>> {
        /// <inheritdoc />
        public KeyedLookup(Func<TValue, TKey> keySelector, IEnumerable<TValue> values) : base(CreateCollection, keySelector, values) { }

        /// <inheritdoc />
        public KeyedLookup(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> equalityComparer = null, IEnumerable<TValue> values = null) : base(CreateCollection, keySelector, equalityComparer, values) { }

        private static List<TValue> CreateCollection() {
            return new List<TValue>();
        }
    }


    /// <summary>
    /// A collection indexed by a non unique key
    /// </summary>
    /// <typeparam name="TKey">They key</typeparam>
    /// <typeparam name="TValue">The value</typeparam>
    /// <typeparam name="TCollection">The value</typeparam>
    public class KeyedLookup<TKey, TValue, TCollection> : ILookup<TKey, TValue> where TCollection : ICollection<TValue> {
		/// <summary>
		/// The internal dictionary
		/// </summary>
		protected MutableLookup<TKey, TValue, TCollection> InnerLookup { get; }

		/// <summary>
		/// The selector method
		/// </summary>
		protected Func<TValue, TKey> KeySelector { get; }

        /// <summary>
        /// Get a value by its key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        public TCollection this[TKey key] => InnerLookup[key];

        /// <summary>
        /// Get a value by its key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] => InnerLookup[key];

        /// <summary>
        /// Creates a new instance of <see cref="KeyedLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        /// <param name="instantiateFunc"></param>
        /// <param name="keySelector">The key selector</param>
        /// <param name="values">The initial values</param>
        public KeyedLookup(Func<TCollection> instantiateFunc, Func<TValue, TKey> keySelector, IEnumerable<TValue> values)
        :this(instantiateFunc, keySelector, null, values){
		}

        /// <summary>
        /// Creates a new instance of <see cref="KeyedLookup{TKey,TValue,TCollection}"/>
        /// </summary>
        /// <param name="instantiateFunc"></param>
        /// <param name="keySelector">The key selector</param>
        /// <param name="equalityComparer">The equality comparer</param>
        /// <param name="values">The initial values</param>
        public KeyedLookup(Func<TCollection> instantiateFunc, Func<TValue, TKey> keySelector, IEqualityComparer<TKey> equalityComparer = null, IEnumerable<TValue> values = null) {
			KeySelector = keySelector;
			InnerLookup = new MutableLookup<TKey, TValue, TCollection>(equalityComparer, instantiateFunc);
            if(values != null) {
                foreach (var value in values) {
                    InnerLookup.Add(keySelector.Invoke(value), value);
                }
            }
        }

		/// <inheritdoc />

		/// <summary>
		/// The keys of the collection
		/// </summary>
		public IEnumerable<TKey> Keys => InnerLookup.Keys.ToList();
        
        /// <summary>
		/// Get the amount of items in the collection
		/// </summary>
		public int Count => InnerLookup.Count;
        
		/// <inheritdoc />
		public void CopyTo(TValue[] array, int arrayIndex) {
			InnerLookup.Values.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Add an item to the collection
		/// </summary>
		/// <param name="value"></param>
		public void Add(TValue value) {
            InnerLookup.Add(KeySelector.Invoke(value), value);
        }

		/// <summary>
		/// Add a range of items to the collection
		/// </summary>
		/// <param name="values"></param>
		public void AddRange(IEnumerable<TValue> values) {
            foreach(var value in values) {
                InnerLookup.Add(KeySelector.Invoke(value), value);
            }
        }

        /// <summary>
        /// Tries to remove a value from the collection
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Remove(TValue value) {
            return InnerLookup.Remove(KeySelector.Invoke(value), value);
        }

        /// <summary>
        /// Checks if the collection contains a value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(TValue value) {
            return InnerLookup.Contains(KeySelector.Invoke(value));
        }

        /// <summary>
        /// Checks if the collection contains a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key) {
            return InnerLookup.Contains(key);
        }

		/// <summary>
		/// Removes all values from the collection
		/// </summary>
		public void Clear() { InnerLookup.Clear(); }

        public bool TryGetValue(TKey key, out TCollection values) {
            return InnerLookup.TryGetValue(key, out values);
        }

        /// <inheritdoc />
        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator() {
            return InnerLookup.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
