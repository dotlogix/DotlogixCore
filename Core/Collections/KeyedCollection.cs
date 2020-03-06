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
using DotLogix.Core.Extensions;

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
		public TValue this[TKey key] => InnerDictionary[key];

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

		/// <inheritdoc />

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

		public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)InnerDictionary).IsReadOnly;

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

		#region ICollection of object

		public void Add(object item) {
			if (!(item is TValue value))
				throw new Exception($"The value is not assignable to target type {typeof(TValue).GetFriendlyName()}");
			if (TryAdd(value) == false)
				throw new Exception("A value with this key already exists");
		}

		public bool Remove(object item) {
			if (!(item is TValue value))
				throw new Exception($"The value is not assignable to target type {typeof(TValue).GetFriendlyName()}");
			return Remove(value);
		}

		public bool Contains(object item) {
			if (!(item is TValue value))
				throw new Exception($"The value is not assignable to target type {typeof(TValue).GetFriendlyName()}");
			return Contains(value);
		}

		#endregion

		/// <summary>
		/// Tries to add a value to the collection
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryAdd(TValue value) { return InnerDictionary.TryAdd(KeySelector.Invoke(value), value); }

		/// <summary>
		/// Tries to add a value to the collection
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public void TryAdd(IEnumerable<TValue> values) {
			foreach (var value in values)
				TryAdd(value);
		}

		/// <summary>
		/// Get the existing item or adds a new one matched by their keys
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public TValue GetOrAdd(TValue value) { return InnerDictionary.GetOrAdd(KeySelector.Invoke(value), value); }

		/// <summary>
		/// Get the existing item or adds a new one matched by their keys
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public IEnumerable<TValue> GetOrAdd(IEnumerable<TValue> values) { return values.Select(GetOrAdd).ToList(); }

		/// <summary>
		/// Add or update a value to the collection
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public void AddOrUpdate(TValue value) { InnerDictionary[KeySelector.Invoke(value)] = value; }

		/// <summary>
		/// Add or update a value to the collection
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public void AddOrUpdate(IEnumerable<TValue> values) {
			foreach (var value in values)
				AddOrUpdate(value);
		}

		/// <summary>
		/// Tries to get a value by key
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGet(TKey key, out TValue value) { return InnerDictionary.TryGetValue(key, out value); }

		/// <summary>
		/// Tries to remove the value with the matching key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool TryRemoveKey(TKey key) { return InnerDictionary.TryRemove(key, out var _); }

		/// <summary>
		/// Tries to remove the value with the matching key
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public bool TryRemoveKey(IEnumerable<TKey> keys) {
			return keys.Aggregate(true, (current, value) => current & TryRemoveKey(value));
		}

		/// <summary>
		/// Checks if a key exists in the collection
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(TKey key) { return InnerDictionary.ContainsKey(key); }
	}
}
