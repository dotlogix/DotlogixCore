// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  KeyedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
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
		/// The selector method
		/// </summary>
		protected Func<TValue, TKey> KeySelector { get; }

		/// <summary>
		/// Get a value by its key
		/// </summary>
		/// <param name="key">The key</param>
		/// <returns></returns>
		public TValue this[TKey key] => Get(key);

		/// <summary>
		/// Creates a new instance of <see cref="KeyedCollection{TKey,TValue}"/>
		/// </summary>
		/// <param name="keySelector">The key selector</param>
		public KeyedCollection(Func<TValue, TKey> keySelector) {
			KeySelector = keySelector;
			InnerDictionary = new ConcurrentDictionary<TKey, TValue>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="KeyedCollection{TKey,TValue}"/>
		/// </summary>
		/// <param name="keySelector">The key selector</param>
		/// <param name="values">The initial values</param>
		public KeyedCollection(Func<TValue, TKey> keySelector, IEnumerable<TValue> values) {
			KeySelector = keySelector;
			InnerDictionary = new ConcurrentDictionary<TKey, TValue>(values.ToDictionary(keySelector));
		}

		/// <summary>
		/// Creates a new instance of <see cref="KeyedCollection{TKey,TValue}"/>
		/// </summary>
		/// <param name="keySelector">The key selector</param>
		/// <param name="equalityComparer">The equality comparer</param>
		public KeyedCollection(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> equalityComparer) {
			KeySelector = keySelector;
			InnerDictionary = new ConcurrentDictionary<TKey, TValue>(equalityComparer);
		}

		/// <summary>
		/// Creates a new instance of <see cref="KeyedCollection{TKey,TValue}"/>
		/// </summary>
		/// <param name="keySelector">The key selector</param>
		/// <param name="equalityComparer">The equality comparer</param>
		/// <param name="values">The initial values</param>
		public KeyedCollection(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> equalityComparer,
			IEnumerable<TValue> values) {
			KeySelector = keySelector;
			InnerDictionary = new ConcurrentDictionary<TKey, TValue>(values.ToDictionary(keySelector, equalityComparer));
		}

		/// <summary>
		/// The keys of the collection
		/// </summary>
		public IEnumerable<TKey> Keys => InnerDictionary.Keys.ToList();

		/// <summary>
		/// The key value paires of the collection
		/// </summary>
		public IEnumerable<KeyValuePair<TKey, TValue>> Pairs => InnerDictionary.ToList();

		#region ICollection

		/// <summary>
		/// Get the amount of items in the collection
		/// </summary>
		public int Count => InnerDictionary.Count;

		/// <inheritdoc />
		bool ICollection<TValue>.IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)InnerDictionary).IsReadOnly;

		/// <inheritdoc />
		public void CopyTo(TValue[] array, int arrayIndex) {
			InnerDictionary.Values.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Add an item to the collection
		/// </summary>
		/// <param name="value"></param>
		public void Add(TValue value) {
			if (TryAdd(value) == false)
				throw new Exception("A value with this key already exists");
		}

		/// <summary>
		/// Tries to remove a value from the collection
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Remove(TValue value) { return InnerDictionary.TryRemove(KeySelector.Invoke(value), out var _); }

		/// <summary>
		/// Checks if the collection contains a value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(TValue value) { return InnerDictionary.ContainsKey(KeySelector.Invoke(value)); }

		/// <summary>
		/// Removes all values from the collection
		/// </summary>
		public void Clear() { InnerDictionary.Clear(); }

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		/// <inheritdoc />
		public IEnumerator<TValue> GetEnumerator() { return InnerDictionary.Values.GetEnumerator(); }

		#endregion

		/// <summary>
		/// Get a value by key
		/// </summary>
		public TValue Get(TKey key, TValue defaultValue = default) { return InnerDictionary.TryGetValue(key, out var value) ? value : defaultValue; }

		/// <summary>
		/// Get a range of values by key
		/// </summary>
		public IEnumerable<TValue> Get(IEnumerable<TKey> keys) {
			foreach(var key in keys) {
				if(InnerDictionary.TryGetValue(key, out var value)) {
					yield return value;
				}
			}
		}

		/// <summary>
		/// Tries to get a value by key
		/// </summary>
		public bool TryGetValue(TKey key, out TValue value) { return InnerDictionary.TryGetValue(key, out value); }

		/// <summary>
		/// Get an existing item or add a new one if necessary
		/// </summary>
		public TValue GetOrAdd(TValue value) { return InnerDictionary.GetOrAdd(KeySelector.Invoke(value), value); }

		/// <summary>
		/// Get a range of existing items or add a new ones if necessary
		/// </summary>
		public IEnumerable<TValue> GetOrAdd(IEnumerable<TValue> values) { return values.Select(GetOrAdd).ToList(); }

		/// <summary>
		/// Add or update a value to the collection
		/// </summary>
		public void AddOrUpdate(TValue value) { InnerDictionary[KeySelector.Invoke(value)] = value; }

		/// <summary>
		/// Add or update a value to the collection
		/// </summary>
		public void AddOrUpdate(IEnumerable<TValue> values) {
			foreach (var value in values)
				AddOrUpdate(value);
		}

		/// <summary>
		/// Tries to add a value to the collection
		/// </summary>
		public bool TryAdd(TValue value) { return InnerDictionary.TryAdd(KeySelector.Invoke(value), value); }

		/// <summary>
		/// Tries to add a value to the collection
		/// </summary>
		public void TryAdd(IEnumerable<TValue> values) {
			foreach (var value in values)
				TryAdd(value);
		}
		
		/// <summary>
		/// Tries to remove the value with the matching key
		/// </summary>
		public bool TryRemoveKey(TKey key) { return InnerDictionary.TryRemove(key, out var _); }

		/// <summary>
		/// Tries to remove the value with the matching key
		/// </summary>
		public int TryRemoveKey(IEnumerable<TKey> keys) {
			return keys.Count(TryRemoveKey);
		}

		/// <summary>
		/// Checks if a key exists in the collection
		/// </summary>
		public bool ContainsKey(TKey key) { return InnerDictionary.ContainsKey(key); }
	}
}
