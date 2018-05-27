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
    public class Cache<TKey, TValue> : ICache<TKey, TValue> {
        private readonly ConcurrentDictionary<TKey, CacheItem<TKey, TValue>> _cacheItems = new ConcurrentDictionary<TKey, CacheItem<TKey, TValue>>();
        private readonly Timer _cleanupTimer;

        public Cache(int checkPolicyInterval) : this(TimeSpan.FromMilliseconds(checkPolicyInterval)) { }

        public Cache(TimeSpan checkPolicyInterval) {
            CheckPolicyInterval = checkPolicyInterval;
            _cleanupTimer = new Timer(state => Cleanup(), null, CheckPolicyInterval, CheckPolicyInterval);
        }


        public void Dispose() {
            _cleanupTimer.Dispose();
        }

        public TimeSpan CheckPolicyInterval { get; }

        public TValue this[TKey key] => Retrieve(key);

        public bool IsAlive(TKey key) {
            return _cacheItems.ContainsKey(key);
        }

        public void Store(TKey key, TValue value, ICachePolicy policy = null) {
            _cacheItems[key] = new CacheItem<TKey, TValue>(key, value, policy);
        }

        public TValue Retrieve(TKey key) {
            return _cacheItems.TryGetValue(key, out var item) ? item.Value : default(TValue);
        }

        public bool TryRetrieve(TKey key, out TValue value) {
            value = default(TValue);
            if(_cacheItems.TryGetValue(key, out var item) == false)
                return false;
            value = item.Value;
            return true;
        }

        public TValue Pop(TKey key) {
            return _cacheItems.TryRemove(key, out var item) ? item.Value : default(TValue);
        }

        public bool TryPop(TKey key, out TValue value) {
            value = default(TValue);
            if(_cacheItems.TryRemove(key, out var item) == false)
                return false;
            value = item.Value;
            return true;
        }

        public bool Discard(TKey key) {
            if(_cacheItems.TryRemove(key, out var item) == false)
                return false;

            ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(item.ToSingleElementArray()));
            return true;
        }

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

        public void Clear() {
            var discardedItems = _cacheItems.Values.ToList();
            _cacheItems.Clear();
            if (discardedItems.Count > 0)
                ItemsDiscarded?.Invoke(this, new CacheItemsDiscardedEventArgs<TKey, TValue>(discardedItems));
        }

        public event EventHandler<CacheItemsDiscardedEventArgs<TKey, TValue>> ItemsDiscarded;
    }
}
