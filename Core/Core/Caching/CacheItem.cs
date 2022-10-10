// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  CacheItem.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
// ==================================================

using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Caching; 

/// <summary>
///     An item for a cache implementation
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class CacheItem<TKey, TValue> {
    private IDictionary<object, object> _associatedValues;

    /// <summary>
    ///     The key
    /// </summary>
    public TKey Key { get; }

    /// <summary>
    ///     The value
    /// </summary>
    public TValue Value { get; set; }

    private HashSet<TKey> _children;

    /// <summary>
    ///     Check if there are dependent cache keys
    /// </summary>
    public bool HasChildren => _children is { Count: > 0 };
    /// <summary>
    ///     Check if there are dependent cache keys
    /// </summary>
    public bool HasAssociatedValues => _associatedValues is { Count: > 0 };
    /// <summary>
    ///     The dependent keys
    /// </summary>
    public HashSet<TKey> Children => _children ??= new HashSet<TKey>();

    /// <summary>
    ///     Additional values associated with this cache item
    /// </summary>
    public IDictionary<object, object> AssociatedValues => _associatedValues ??= new Dictionary<object, object>();

    /// <summary>
    ///     The policy to check validity
    /// </summary>
    public ICachePolicy Policy { get; set; }

    /// <summary>
    ///     Creates a new instance of <see cref="CacheItem{TKey,TValue}" />
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    /// <param name="policy">The policy to check validity</param>
    public CacheItem(TKey key, TValue value, ICachePolicy policy) {
        Key = key;
        Value = value;
        Policy = policy;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="CacheItem{TKey,TValue}" />
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    /// <param name="policy">The policy to check validity</param>
    /// <param name="dependencies">The keys depending on this item</param>
    public CacheItem(TKey key, TValue value, ICachePolicy policy, IEnumerable<TKey> dependencies) {
        Key = key;
        Value = value;
        Policy = policy;
        if(dependencies is not null)
            _children = dependencies as HashSet<TKey> ?? dependencies.ToHashSet();
    }
}