// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Cache.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Caching {
    /// <summary>
    /// A concurrent version of a cache using policies to determ when values exceed
    /// </summary>
    /// <typeparam name="TKey">The type of the keys</typeparam>
    /// <typeparam name="TValue">The type of the values</typeparam>
    public class Cache<TKey, TValue> : ICache<TKey, TValue> {
        private readonly ConcurrentDictionary<TKey, CacheItem<TKey, TValue>> _cacheItems = new ConcurrentDictionary<TKey, CacheItem<TKey, TValue>>();
        private readonly Timer _cleanupTimer;

        /// <summary>
        /// Creates a new instance of the cache and checks the values in the given intervall
        /// </summary>
        public Cache(int checkPolicyInterval) : this(TimeSpan.FromMilliseconds(checkPolicyInterval)) { }

        /// <summary>
        /// Creates a new instance of the cache and checks the values in the given intervall
        /// </summary>
        public Cache(TimeSpan checkPolicyInterval) {
            CheckPolicyInterval = checkPolicyInterval;
            _cleanupTimer = new Timer(state => Cleanup(), null, CheckPolicyInterval, CheckPolicyInterval);
        }

        /// <summary>
        /// Disposes the cache
        /// </summary>
        public void Dispose() {
            _cleanupTimer.Dispose();
        }

        /// <summary>
        /// The timespan to check if values are no longer valid
        /// </summary>
        public TimeSpan CheckPolicyInterval { get; }

        /// <summary>
        /// Get the value for a given key. Returns default if value can not be found
        /// </summary>
        public TValue this[TKey key] => Retrieve(key);

        /// <summary>
        /// Checks if a value is defined for a given key
        /// </summary>
        public bool IsAlive(TKey key) {
            return _cacheItems.ContainsKey(key);
        }

        /// <summary>
        /// Stores a value with the given key to the cache. Existing values will be overridden
        /// </summary>
        /// <param name="key">The key of the value</param>
        /// <param name="value">The vlaue</param>
        /// <param name="policy">The policy used to check if the value exceed, or null to keep the value until the cache is disposed</param>
        public void Store(TKey key, TValue value, ICachePolicy policy = null) {
            _cacheItems[key] = new CacheItem<TKey, TValue>(key, value, policy);
        }

        /// <summary>
        /// Retrieves a value by its key. Returns default if the key is not present
        /// </summary>
        public TValue Retrieve(TKey key) {
            return _cacheItems.TryGetValue(key, out var item) ? item.Value : default(TValue);
        }

        /// <summary>
        /// Trys to retrieve a value by its key.
        /// </summary>
        public bool TryRetrieve(TKey key, out TValue value) {
            value = default(TValue);
            if(_cacheItems.TryGetValue(key, out var item) == false)
                return false;
            value = item.Value;
            return true;
        }

        /// <summary>
        /// Gets and remove a value by its key. Returns default if the key is not present
        /// </summary>
        public TValue Pop(TKey key) {
            return _cacheItems.TryRemove(key, out var item) ? item.Value : default(TValue);
        }

        /// <summary>
        /// Tries to get and remove a value by its key.
        /// </summary>
        public bool TryPop(TKey key, out TValue value) {
            value = default(TValue);
            if(_cacheItems.TryRemove(key, out var item) == false)
                return false;
            value = item.Value;
            return true;
        }

        /// <summary>
        /// Removes a value by its key
        /// </summary>
        public bool Discard(TKey key) {
            if(_cacheItems.TryRemove(key, out var item) == false)
                return false;

            ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(item.ToSingleElementArray()));
            return true;
        }

        /// <summary>
        /// Forces a revalidation of all items in the cache and removes exeeded items
        /// </summary>
        public void Cleanup() {
            if(_cacheItems.IsEmpty)
                return;

            var utcNow = DateTime.UtcNow;
            var discardedItems = new List<CacheItem<TKey, TValue>>();
            foreach(var cacheItem in _cacheItems.Values) {
                if((cacheItem.Policy == null) || (cacheItem.Policy.HasExpired(utcNow) == false))
                    continue;

                discardedItems.Add(cacheItem);
                _cacheItems.TryRemove(cacheItem.Key, out _);
            }

            if(discardedItems.Count > 0)
                ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(discardedItems));
        }

        /// <summary>
        /// Removes all items of the cache
        /// </summary>
        public void Clear() {
            var discardedItems = _cacheItems.Values.ToList();
            _cacheItems.Clear();
            if (discardedItems.Count > 0)
                ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(discardedItems));
        }

        /// <summary>
        /// Occures when items are discarded in the cache
        /// </summary>
        public event EventHandler<CacheItemsDiscardedEventArgs<TKey, TValue>> ItemsDiscarded;
    }
}
