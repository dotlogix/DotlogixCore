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
	/// A read-only collection indexed by a unique key
	/// </summary>
	public interface IReadOnlyKeyedCollection<TKey, TItem> : IReadOnlyCollection<TItem> {
		/// <summary>
		/// Gets an item by key
		/// </summary>
		TItem this[TKey key] { get; }

		/// <summary>
		/// Get all items as a range of keys
		/// </summary>
		IEnumerable<TKey> Keys { get; }

		/// <summary>
		/// Get all items as a range of <see cref="KeyValuePair{TKey,TItem}"/>
		/// </summary>
		IEnumerable<KeyValuePair<TKey, TItem>> Pairs { get; }

		/// <summary>
		/// Gets an item by key.
		/// If no matching item could be found <paramref name="defaultValue"/> will be returned instead.
		/// </summary>
		TItem Get(TKey key, TItem defaultValue = default);

		/// <summary>
		/// Tries to get a range of items by key.
		/// Keys where not matching item could be found will be ignored.
		/// </summary>
		IEnumerable<TItem> Get(IEnumerable<TKey> keys);

		/// <summary>
		/// Tries to get an item by key
		/// </summary>
		bool TryGetValue(TKey key, out TItem item);

		/// <summary>
		/// Determines if the provided item exists
		/// </summary>
		bool Contains(TItem item);
		
		/// <summary>
		/// Determines if an item with the provided key exists
		/// </summary>
		bool ContainsKey(TKey key);

		/// <summary>
		/// Copies all items of to an Array, starting at a particular Array index.
		/// </summary>
		void CopyTo(TItem[] array, int arrayIndex);
	}
	
	/// <summary>
	/// A collection indexed by a unique key
	/// </summary>
	public interface IKeyedCollection<TKey, TItem> : IReadOnlyKeyedCollection<TKey, TItem>, ICollection<TItem> {
		/// <summary>
		/// Gets or adds an item with same key as <paramref name="value"/>.
		/// </summary>
		public TItem GetOrAdd(TItem value);

		/// <summary>
		/// Gets or adds a range of items with same keys as <paramref name="values"/>.
		/// </summary>
		public IEnumerable<TItem> GetOrAdd(IEnumerable<TItem> values);

		/// <summary>
		/// Adds or updates an item with same key as <paramref name="value"/>.
		/// </summary>
		public void AddOrUpdate(TItem value);

		/// <summary>
		/// Adds or updates a range of items with same keys as <paramref name="values"/>.
		/// </summary>
		public void AddOrUpdate(IEnumerable<TItem> values);

		/// <summary>
		/// Tries to add an item
		/// </summary>
		public bool TryAdd(TItem item);

		/// <summary>
		/// Tries to add a range of items
		/// </summary>
		public int TryAdd(IEnumerable<TItem> items);

		/// <summary>
		/// Tries to remove an item
		/// </summary>
		public bool TryRemove(TItem item);

		/// <summary>
		/// Tries to remove a range of items
		/// </summary>
		public int TryRemove(IEnumerable<TItem> items);
		
		/// <summary>
		/// Tries to remove an item matching the provided key
		/// </summary>
		public bool TryRemoveKey(TKey key);

		/// <summary>
		/// Tries to remove all items matching the provided keys
		/// </summary>
		public int TryRemoveKey(IEnumerable<TKey> keys);
	}
	
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
