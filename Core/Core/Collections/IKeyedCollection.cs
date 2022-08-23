using System.Collections.Generic;

namespace DotLogix.Core.Collections; 

/// <summary>
/// A collection indexed by a unique key
/// </summary>
public interface IKeyedCollection<TKey, TItem> : IReadOnlyKeyedCollection<TKey, TItem>, ICollection<TItem> {
    /// <summary>
    /// Gets or adds an item with same key as <paramref name="value"/>.
    /// </summary>
    TItem GetOrAdd(TItem value);

    /// <summary>
    /// Gets or adds a range of items with same keys as <paramref name="values"/>.
    /// </summary>
    IEnumerable<TItem> GetOrAdd(IEnumerable<TItem> values);

    /// <summary>
    /// Adds or updates an item with same key as <paramref name="value"/>.
    /// </summary>
    void AddOrUpdate(TItem value);

    /// <summary>
    /// Adds or updates a range of items with same keys as <paramref name="values"/>.
    /// </summary>
    void AddOrUpdate(IEnumerable<TItem> values);

    /// <summary>
    /// Tries to add an item
    /// </summary>
    bool TryAdd(TItem item);

    /// <summary>
    /// Tries to add a range of items
    /// </summary>
    int TryAdd(IEnumerable<TItem> items);

    /// <summary>
    /// Tries to remove an item
    /// </summary>
    bool TryRemove(TItem item);

    /// <summary>
    /// Tries to remove a range of items
    /// </summary>
    int TryRemove(IEnumerable<TItem> items);
		
    /// <summary>
    /// Tries to remove an item matching the provided key
    /// </summary>
    bool TryRemoveKey(TKey key);

    /// <summary>
    /// Tries to remove all items matching the provided keys
    /// </summary>
    int TryRemoveKey(IEnumerable<TKey> keys);
}