﻿// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  Cache.cs
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
using System.Threading;
#endregion

namespace DotLogix.Core.Caching; 

/// <summary>
///     A concurrent version of a cache using policies to determ when values exceed
/// </summary>
/// <typeparam name="TKey">The type of the keys</typeparam>
/// <typeparam name="TValue">The type of the values</typeparam>
public class Cache<TKey, TValue> : ICache<TKey, TValue> {
    private readonly ConcurrentDictionary<TKey, CacheItem<TKey, TValue>> _cacheItems;
    private readonly Timer _cleanupTimer;

    /// <summary>
    ///     Creates a new instance of the cache and checks the values in the given intervall
    /// </summary>
    public Cache(int checkPolicyInterval, IEqualityComparer<TKey> comparer = null) : this(TimeSpan.FromMilliseconds(checkPolicyInterval), comparer) { }

    /// <summary>
    ///     Creates a new instance of the cache and checks the values in the given intervall
    /// </summary>
    public Cache(TimeSpan checkPolicyInterval, IEqualityComparer<TKey> comparer = null) {
        CheckPolicyInterval = checkPolicyInterval;
        _cacheItems = new ConcurrentDictionary<TKey, CacheItem<TKey, TValue>>(comparer ?? EqualityComparer<TKey>.Default);
        _cleanupTimer = new Timer(state => Cleanup(), null, CheckPolicyInterval, CheckPolicyInterval);
    }

    /// <summary>
    ///     Disposes the cache
    /// </summary>
    public void Dispose() {
        _cleanupTimer.Dispose();
    }

    /// <summary>
    ///     The timespan to check if values are no longer valid
    /// </summary>
    public TimeSpan CheckPolicyInterval { get; }

    /// <summary>
    ///     The current amount of items
    /// </summary>
    public int Count => _cacheItems.Count;

    /// <summary>
    ///     Get the value for a given key. Returns default if value can not be found
    /// </summary>
    public TValue this[TKey key] => Retrieve(key);

    /// <summary>
    ///     Checks if a value is defined for a given key
    /// </summary>
    public bool IsAlive(TKey key) {
        return _cacheItems.ContainsKey(key);
    }

    /// <summary>
    ///     Stores a value with the given key to the cache. Existing values will be overridden
    /// </summary>
    /// <param name="key">The key of the value</param>
    /// <param name="value">The vlaue</param>
    /// <param name="policy">
    ///     The policy used to check if the value exceed, or null to keep the value until the cache is
    ///     disposed
    /// </param>
    /// <param name="preserveContext">Determines if dependencies and associations should be preserved</param>
    public CacheItem<TKey, TValue> Store(TKey key, TValue value, ICachePolicy policy = null, bool preserveContext = true) {
        CacheItem<TKey, TValue> AddValueFactory(TKey k) {
            return new CacheItem<TKey, TValue>(key, value, policy);
        }

        CacheItem<TKey, TValue> UpdateValueFactory(TKey k, CacheItem<TKey, TValue> existing) {
            if(preserveContext == false)
                return new CacheItem<TKey, TValue>(key, value, policy);

            existing.Value = value;
            existing.Policy = policy;
            return existing;
        }

        return _cacheItems.AddOrUpdate(key, AddValueFactory, UpdateValueFactory);
    }

    /// <summary>
    ///     Retrieves a value by its key. Creates one if the key is not present
    /// </summary>
    public TValue RetrieveOrCreate(TKey key, Func<TKey, TValue> createFunc, ICachePolicy policy = null, bool updatePolicy = true) {
        return RetrieveOrCreateItem(key, createFunc, policy, updatePolicy).Value;
    }

    /// <summary>
    ///     Retrieves a value by its key. Creates one if the key is not present
    /// </summary>
    public TValue RetrieveOrCreate(TKey key, TValue value, ICachePolicy policy = null, bool updatePolicy = true) {
        return RetrieveOrCreateItem(key, value, policy, updatePolicy).Value;
    }


    /// <summary>
    ///     Retrieves a value by its key. Returns default if the key is not present
    /// </summary>
    public TValue Retrieve(TKey key, TValue defaultValue = default) {
        return _cacheItems.TryGetValue(key, out var item) ? item.Value : defaultValue;
    }

    /// <summary>
    ///     Tries to retrieve a value by its key.
    /// </summary>
    public bool TryRetrieve(TKey key, out TValue value) {
        value = default;
        if (_cacheItems.TryGetValue(key, out var item) == false)
            return false;
        value = item.Value;
        return true;
    }

    /// <summary>
    ///     Gets and remove a value by its key. Returns default if the key is not present
    /// </summary>
    public TValue Pop(TKey key, TValue defaultValue = default) {
        return TryPop(key, out var value)
            ? value
            : defaultValue;
    }

    /// <summary>
    ///     Tries to get and remove a value by its key.
    /// </summary>
    public bool TryPop(TKey key, out TValue value) {
        value = default;
        if(_cacheItems.TryGetValue(key, out var item) == false || Discard(key) == false)
            return false;

        value = item.Value;
        return true;


    }

    /// <summary>
    ///     Removes a value by its key
    /// </summary>
    public bool Discard(TKey key) {
        var discardedItems = new Dictionary<TKey, CacheItemDiscardedEventArgs<TKey, TValue>>();
        DiscardRecursive(key, CacheItemDiscardReason.Discarded, discardedItems);

        if (discardedItems.Count <= 0)
            return false;

        ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(discardedItems.Values.ToList(), CacheItemDiscardReason.Discarded));
        return true;
    }

    /// <summary>
    ///     Removes a value by its key
    /// </summary>
    public bool DiscardChildren(TKey key) {
        var discardedItems = new Dictionary<TKey, CacheItemDiscardedEventArgs<TKey, TValue>>();
        if(_cacheItems.TryGetValue(key, out var item)) {
            if(item.HasChildren) {
                foreach(var dependentKey in item.Children) {
                    DiscardRecursive(dependentKey, CacheItemDiscardReason.Discarded, discardedItems);
                }
                item.Children.Clear();
            }
        }

        if (discardedItems.Count <= 0)
            return false;

        ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(discardedItems.Values.ToList(), CacheItemDiscardReason.Discarded));
        return true;
    }

    /// <summary>
    ///     Retrieves a cache item by its key. Returns null if the key is not present
    /// </summary>
    public CacheItem<TKey, TValue> RetrieveItem(TKey key) {
        return _cacheItems.TryGetValue(key, out var item) ? item : null;
    }

    /// <summary>
    ///     Tries to retrieve a cache item by its key.
    /// </summary>
    public bool TryRetrieveItem(TKey key, out CacheItem<TKey, TValue> item) {
        return _cacheItems.TryGetValue(key, out item);
    }

    /// <summary>
    ///     Retrieves a value by its key. Creates one if the key is not present
    /// </summary>
    public CacheItem<TKey, TValue> RetrieveOrCreateItem(TKey key, Func<TKey, TValue> createFunc, ICachePolicy policy = null, bool updatePolicy = true) {
        CacheItem<TKey, TValue> AddValueFactory(TKey k) {
            var value = createFunc.Invoke(k);
            return new CacheItem<TKey, TValue>(k, value, policy);
        }

        CacheItem<TKey, TValue> UpdateValueFactory(TKey k, CacheItem<TKey, TValue> existing) {
            existing.Policy = policy;
            return existing;
        }

        if (updatePolicy)
            return _cacheItems.AddOrUpdate(key, AddValueFactory, UpdateValueFactory);
        return _cacheItems.GetOrAdd(key, AddValueFactory);
    }

    /// <summary>
    ///     Retrieves a value by its key. Creates one if the key is not present
    /// </summary>
    public CacheItem<TKey, TValue> RetrieveOrCreateItem(TKey key, TValue value, ICachePolicy policy = null, bool updatePolicy = true) {
        CacheItem<TKey, TValue> AddValueFactory(TKey k) {
            return new CacheItem<TKey, TValue>(k, value, policy);
        }

        CacheItem<TKey, TValue> UpdateValueFactory(TKey k, CacheItem<TKey, TValue> existing) {
            existing.Policy = policy;
            return existing;
        }

        if (updatePolicy)
            return _cacheItems.AddOrUpdate(key, AddValueFactory, UpdateValueFactory);
        return _cacheItems.GetOrAdd(key, AddValueFactory);
    }

    /// <summary>
    ///     Forces a re validation of all items in the cache and removes exceeded items
    /// </summary>
    public void Cleanup() {
        if(_cacheItems.IsEmpty)
            return;

        var utcNow = DateTime.UtcNow;


        var discardedItems = new Dictionary<TKey, CacheItemDiscardedEventArgs<TKey, TValue>>();
        foreach (var cacheItem in _cacheItems.Values) {
            if(discardedItems.ContainsKey(cacheItem.Key))
                continue;

            if((cacheItem.Policy == null) || (cacheItem.Policy.HasExpired(utcNow) == false))
                continue;

            DiscardRecursive(cacheItem.Key, CacheItemDiscardReason.Expired, discardedItems);
        }

        if(discardedItems.Count > 0)
            ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(discardedItems.Values.ToList()));
    }

    /// <summary>
    ///     Removes all items of the cache
    /// </summary>
    public void Clear() {
        var discardedItems = new Dictionary<TKey, CacheItemDiscardedEventArgs<TKey, TValue>>();
        foreach (var key in _cacheItems.Keys) {
            if (discardedItems.ContainsKey(key))
                continue;
            DiscardRecursive(key, CacheItemDiscardReason.Expired, discardedItems);
        }

        if (discardedItems.Count > 0)
            OnItemsDiscarded(discardedItems.Values.ToList());
    }

    /// <summary>
    ///     Occures when items are discarded in the cache
    /// </summary>
    public event EventHandler<CacheItemsDiscardedEventArgs<TKey, TValue>> ItemsDiscarded;

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<CacheItem<TKey, TValue>> GetEnumerator() {
        return _cacheItems.Values.ToList().GetEnumerator();
    }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    #region Helper

    private void OnItemsDiscarded(IReadOnlyList<CacheItemDiscardedEventArgs<TKey, TValue>> discardedItems) {
        ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(discardedItems));
    }

    private CacheItemDiscardedEventArgs<TKey, TValue> DiscardRecursive(TKey key, CacheItemDiscardReason reason, IDictionary<TKey, CacheItemDiscardedEventArgs<TKey, TValue>> discardedItems) {
        if(discardedItems.TryGetValue(key, out var currentArgs))
            return currentArgs;

        if (_cacheItems.TryRemove(key, out var item) == false) {
            return null;
        }

        currentArgs = new CacheItemDiscardedEventArgs<TKey, TValue>(item, reason);
        discardedItems.Add(key, currentArgs);


        if(item.HasChildren == false)
            return currentArgs;

        foreach(var dependentKey in item.Children) {
            var dependencyItem = DiscardRecursive(dependentKey, reason, discardedItems);

            dependencyItem.Ancestors.Add(currentArgs);
            currentArgs.Dependencies.Add(dependencyItem);
        }

        return currentArgs;
    }

    #endregion
}