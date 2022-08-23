using System.Collections.Generic;

namespace DotLogix.Core.Collections; 

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