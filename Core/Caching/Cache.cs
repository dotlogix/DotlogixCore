// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Cache.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  09.02.2018
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Caching {
    public class Cache<TKey> : ICache<TKey> {
        private readonly ConcurrentDictionary<TKey, CacheItem<TKey>> _cacheDict = new ConcurrentDictionary<TKey, CacheItem<TKey>>();
        private readonly Timer _timer;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public Cache(TimeSpan checkPolicyInterval) {
            CheckPolicyInterval = checkPolicyInterval;
            _timer = new Timer(CheckPolicy, null, checkPolicyInterval, checkPolicyInterval);
        }

        public TimeSpan CheckPolicyInterval { get; }

        public void Cleanup() {
            var utcNow = DateTime.UtcNow;
            var removedValues = new List<object>();

            foreach(var item in _cacheDict.Values) {
                if((item.Policy == null) || (item.Policy.HasExpired(utcNow) == false))
                    continue;

                removedValues.Add(item.Value);
                _cacheDict.TryRemove(item.Key, out _);
                Remove(item.Key);
            }

            if(removedValues.Count > 0)
                ValuesRemoved?.Invoke(this, new CacheCleanupEventArgs(removedValues));
        }

        public object this[TKey key] => Retrieve(key);

        public bool Contains(TKey key) {
            return _cacheDict.ContainsKey(key);
        }

        public void Store(TKey key, object value, ICachePolicy policy = null) {
            var item = new CacheItem<TKey>(key, value, policy);
            _cacheDict[key] = item;
        }

        public TValue Retrieve<TValue>(TKey key) {
            return TryRetrieve(key, out TValue value) ? value : default(TValue);
        }

        public object Retrieve(TKey key) {
            return TryRetrieve(key, out var value) ? value : null;
        }

        public TValue Pop<TValue>(TKey key) {
            return Pop(key).TryConvertTo(out TValue value) ? value : default(TValue);
        }

        public object Pop(TKey key) {
            return _cacheDict.TryRemove(key, out var item) ? item.Value : null;
        }

        public bool Remove(TKey key) {
            return _cacheDict.TryRemove(key, out _);
        }

        public bool TryRetrieve<TValue>(TKey key, out TValue value) {
            value = default(TValue);
            return TryRetrieve(key, out var valueAsObject) && valueAsObject.TryConvertTo(out value);
        }

        public bool TryRetrieve(TKey key, out object value) {
            value = null;

            if(_cacheDict.TryGetValue(key, out var item) == false)
                return false;

            value = item.Value;
            return true;
        }

        /// <inheritdoc />
        public void Dispose() {
            _timer?.Dispose();
        }

        public event EventHandler<CacheCleanupEventArgs> ValuesRemoved;

        private void CheckPolicy(object state) {
            Cleanup();
        }
    }
}
