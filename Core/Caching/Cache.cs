using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Caching {
    public class Cache<TKey> : ICache<TKey> {
        private readonly object _syncRoot = new object();
        private readonly Dictionary<TKey, object> _cacheDict = new Dictionary<TKey, object>();
        private readonly Dictionary<TKey, ICachePolicy> _policyDict = new Dictionary<TKey, ICachePolicy>();
        private readonly Timer _timer;
        public TimeSpan CheckPolicyInterval { get; }

        /// <summary>
        ///   Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public Cache(TimeSpan checkPolicyInterval) {
            CheckPolicyInterval = checkPolicyInterval;
            _timer = new Timer(CheckPolicy, null, -1, -1);
        }

        private void CheckPolicy(object state) {
            var utcNow = DateTime.UtcNow;
            lock(_syncRoot) {
                var policies = _policyDict.ToList();
                foreach (var policy in policies)
                {
                    if (policy.Value.HasExpired(utcNow))
                    {
                        Remove(policy.Key);
                    }
                }
            }
        }

        public object this[TKey key] => Retrieve(key);

        public bool Contains(TKey key) {
            lock (_syncRoot)
            {
                return _cacheDict.ContainsKey(key); 
            }
        }

        public void Store(TKey key, object value, ICachePolicy policy = null) {
            lock (_syncRoot)
            {
                Remove(key);
                _cacheDict.Add(key, value);
                if(policy == null)
                    return;

                _policyDict.Add(key, policy);
                if(_policyDict.Count == 1)
                    _timer.Change(CheckPolicyInterval, CheckPolicyInterval);
            }
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
            lock (_syncRoot)
            {
                if (TryRetrieve(key, out var value) == false)
                    return null;
                Remove(key);
                return value; 
            }
        }

        public bool Remove(TKey key) {
            lock (_syncRoot)
            {
                _policyDict.Remove(key);
                if(_policyDict.Count == 0)
                    _timer.Change(-1, -1);
                return _cacheDict.Remove(key); 
            }
        }

        public bool TryRetrieve<TValue>(TKey key, out TValue value) {
            value = default(TValue);
            return TryRetrieve(key, out var valueAsObject) && valueAsObject.TryConvertTo(out value);
        }

        public bool TryRetrieve(TKey key, out object value) {
            lock (_syncRoot)
            {
                if (_cacheDict.TryGetValue(key, out value) == false)
                    return false;
                if (_policyDict.TryGetValue(key, out var policy) == false || policy.HasExpired(DateTime.UtcNow) == false)
                    return true;
                Remove(key);
                return false; 
            }
        }

        /// <inheritdoc />
        public void Dispose() {
            _timer?.Dispose();
        }
    }
}