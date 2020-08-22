using System;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Utils {
    public class ArrayPool<T>
    {
        private readonly List<T[]> _instances;
        private readonly object _lock = new object();

        public ArrayPool(int capacity = 20, int minArrayLength = 16, int initialCount = 0, bool clearReturned = false)
        {
            Capacity = capacity;
            MinArrayLength = minArrayLength;
            ClearReturned = clearReturned;
            _instances = new List<T[]>(capacity);

            for (var i = 0; i < initialCount; i++)
            {
                _instances.Add(new T[minArrayLength]);
            }
        }

        public int Count => _instances.Count;

        public int Capacity { get; }
        public int MinArrayLength { get; }
        public bool ClearReturned { get; }

        public T[] Rent(int minimumLength) {
            if (TryRent(minimumLength, out var array))
                return array;
            
            throw new InvalidOperationException("Can not retrieve an item from an empty pool");
        }
        
        public T[] RentOrCreate(int minimumLength) {
            if (TryRent(minimumLength, out var array))
                return array;

            return new T[Math.Max(MinArrayLength, minimumLength)];
        }

        public bool TryRent(int minimumLength, out T[] array) {
            lock (_lock) {
                for (var i = _instances.Count - 1; i >= 0; i--) {
                    var instance = _instances[i];
                    if (instance.Length < minimumLength)
                        continue;

                    _instances.RemoveAt(i);
                    {
                        array = instance;
                        return true;
                    }
                }
            }

            array = default;
            return false;
        }

        public bool Return(T[] array)
        {
            lock (_lock) {
                if(Capacity > _instances.Count) {
                    if (ClearReturned)
                        Array.Clear(array, 0, array.Length);

                    _instances.Add(array);
                    return true;
                }

                for (var i = 0; i < _instances.Count; i++) {
                    if (_instances[i].Length >= array.Length)
                        continue;

                    if (ClearReturned)
                        Array.Clear(array, 0, array.Length);
                    _instances[i] = array;
                    return true;
                }
            }

            return false;
        }

        public int Return(IEnumerable<T[]> items)
        {
            return items.Count(Return);
        }
    }
}