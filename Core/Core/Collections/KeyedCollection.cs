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
	public class KeyedCollection<TKey, TValue> : IKeyedCollection<TKey, TValue> {
		/// <summary>
		/// The internal dictionary
		/// </summary>
		protected ConcurrentDictionary<TKey, TValue> InnerDictionary { get; }

		/// <summary>
		/// The selector method
		/// </summary>
		protected Func<TValue, TKey> KeySelector { get; }

		/// <inheritdoc />
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

		/// <inheritdoc />
		public IEnumerable<TKey> Keys => InnerDictionary.Keys.ToList();

		/// <inheritdoc />
		public IEnumerable<KeyValuePair<TKey, TValue>> Pairs => InnerDictionary.ToList();

		#region ICollection

		/// <summary>
		/// Gets the number of items in the collection.
		/// </summary>
		public int Count => InnerDictionary.Count;

		/// <inheritdoc />
		bool ICollection<TValue>.IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)InnerDictionary).IsReadOnly;

		/// <inheritdoc cref="IReadOnlyKeyedCollection{TKey,TItem}.CopyTo" />
		public void CopyTo(TValue[] array, int arrayIndex) {
			InnerDictionary.Values.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc cref="IKeyedCollection{TKey,TItem}.TryAdd(TItem)"/>
		public void Add(TValue value) {
			if (TryAdd(value) == false)
				throw new Exception("A value with this key already exists");
		}

		/// <inheritdoc cref="IKeyedCollection{TKey,TItem}.TryRemove(TItem)"/>
		public bool Remove(TValue value) {
			return InnerDictionary.TryRemove(KeySelector.Invoke(value), out _);
		}

		/// <inheritdoc cref="IReadOnlyKeyedCollection{TKey,TItem}.Contains"/>
		public bool Contains(TValue value) { return InnerDictionary.ContainsKey(KeySelector.Invoke(value)); }

		/// <summary>
		/// Removes all items
		/// </summary>
		public void Clear() { InnerDictionary.Clear(); }

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		/// <inheritdoc />
		public IEnumerator<TValue> GetEnumerator() { return InnerDictionary.Values.GetEnumerator(); }

		#endregion

		/// <inheritdoc />
		public TValue Get(TKey key, TValue defaultValue = default) { return InnerDictionary.TryGetValue(key, out var value) ? value : defaultValue; }

		/// <inheritdoc />
		public IEnumerable<TValue> Get(IEnumerable<TKey> keys) {
			foreach(var key in keys) {
				if(InnerDictionary.TryGetValue(key, out var value)) {
					yield return value;
				}
			}
		}

		/// <inheritdoc />
		public bool TryGetValue(TKey key, out TValue value) { return InnerDictionary.TryGetValue(key, out value); }

		/// <inheritdoc />
		public TValue GetOrAdd(TValue value) { return InnerDictionary.GetOrAdd(KeySelector.Invoke(value), value); }

		/// <inheritdoc />
		public IEnumerable<TValue> GetOrAdd(IEnumerable<TValue> values) { return values.Select(GetOrAdd).ToList(); }

		/// <inheritdoc />
		public void AddOrUpdate(TValue value) { InnerDictionary[KeySelector.Invoke(value)] = value; }

		/// <inheritdoc />
		public void AddOrUpdate(IEnumerable<TValue> values) {
			foreach (var value in values)
				AddOrUpdate(value);
		}

		/// <inheritdoc />
		public bool TryAdd(TValue value) {
			return InnerDictionary.TryAdd(KeySelector.Invoke(value), value);
		}

		/// <inheritdoc />
		public int TryAdd(IEnumerable<TValue> values) {
			return values.Count(TryAdd);
		}

		/// <inheritdoc />
		public bool TryRemove(TValue value) {
			return ((ICollection<KeyValuePair<TKey, TValue>>)InnerDictionary).Remove(new KeyValuePair<TKey,TValue>(KeySelector.Invoke(value), value));
		}
		
		/// <inheritdoc />
		public int TryRemove(IEnumerable<TValue> values) {
			return values.Count(TryRemove);
		}

		/// <inheritdoc />
		public bool TryRemoveKey(TKey key) {
			return InnerDictionary.TryRemove(key, out _);
		}

		/// <inheritdoc />
		public int TryRemoveKey(IEnumerable<TKey> keys) {
			return keys.Count(TryRemoveKey);
		}

		/// <inheritdoc />
		public bool ContainsKey(TKey key) { return InnerDictionary.ContainsKey(key); }
	}
}