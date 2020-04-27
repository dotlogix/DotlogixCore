using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotLogix.Core.Collections {
    public class LinearDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
        private List<KeyValuePair<TKey, TValue>> _pairs = new List<KeyValuePair<TKey, TValue>>();
        private IEqualityComparer<TKey> _comparer;

        public LinearDictionary()
        {
            _comparer = EqualityComparer<TKey>.Default;
        }

        public LinearDictionary(IEqualityComparer<TKey> comparer)
        {
            _comparer = comparer;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if(IndexOfKey(item.Key) >= 0)
                throw new ArgumentException("Key already exists");
            _pairs.Add(item);
        }

        public void Clear()
        {
            _pairs.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            var index = IndexOfKey(item.Key);
            if (index < 0)
                return false;
            return Equals(item.Value, _pairs[index].Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _pairs.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            var index = IndexOfKey(item.Key);
            if (index < 0)
                return false;

            if(Equals(item.Value, _pairs[index].Value))
            {
                _pairs.RemoveAt(index);
                return true;
            }
            return false;
        }

        public int Count => _pairs.Count;
        public bool IsReadOnly { get; } = false;
        public void Add(TKey key, TValue value)
        {
            if(IndexOfKey(key) >= 0)
                throw new ArgumentException("Key already exists");
            _pairs.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool ContainsKey(TKey key)
        {
            return IndexOfKey(key) >= 0;
        }

        private int IndexOfKey(TKey key)
        {
            for (var i = 0; i < _pairs.Count; i++)
            {
                if (_comparer.Equals(_pairs[i].Key, key))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Remove(TKey key)
        {
            var index = IndexOfKey(key);
            if (index < 0)
                return false;

            _pairs.RemoveAt(index);
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var index = IndexOfKey(key);
            if (index < 0)
            {
                value = default;
                return false;
            }

            value = _pairs[index].Value;
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                var index = IndexOfKey(key);
                if (index < 0)
                {
                    throw new KeyNotFoundException();
                }

                return _pairs[index].Value;
            }
            set
            {
                var index = IndexOfKey(key);
                if (index < 0)
                {
                    _pairs.Add(new KeyValuePair<TKey, TValue>(key, value));
                    return;
                }
                _pairs[index] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public ICollection<TKey> Keys => _pairs.Select(p => p.Key).ToList();
        public ICollection<TValue> Values => _pairs.Select(p => p.Value).ToList();
    }
}
