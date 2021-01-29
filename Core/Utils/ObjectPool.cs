using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Utils {
    public class ObjectPool<T> {
        private readonly ConcurrentQueue<T> _items;
        private IInstantiator<T> _instantiator { get; }
        private Func<T, T> _resetItemFunc { get; }
        public int Capacity { get; }
        public int Count => _items.Count;

        public ObjectPool(int capacity, Func<T, T> resetItemFunc = null) {
            Capacity = capacity;
            _instantiator = Instantiator.UseDefaultCtor<T>();
            _resetItemFunc = resetItemFunc;
            _items = new ConcurrentQueue<T>();
        }

        public ObjectPool(int capacity, Func<T> createItemFunc, Func<T, T> resetItemFunc = null) {
            if(createItemFunc == null)
                throw new ArgumentNullException(nameof(createItemFunc));

            Capacity = capacity;
            _instantiator = Instantiator.UseDelegate(createItemFunc);
            _resetItemFunc = resetItemFunc;
            _items = new ConcurrentQueue<T>();
        }

        public ObjectPool(int capacity, IInstantiator<T> instantiator, Func<T, T> resetItemFunc = null) {
            Capacity = capacity;
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
            _resetItemFunc = resetItemFunc;
            _items = new ConcurrentQueue<T>();
        }

        public T Rent() {
            if(_items.IsEmpty || _items.TryDequeue(out var item) == false)
                throw new InvalidOperationException("Can not retrieve an item from an empty pool");
            return item;
        }

        public T RentOrDefault(T defaultValue = default) {
            return _items.TryDequeue(out var item) ? item : defaultValue;
        }

        public bool TryRent(out T item)
        {
            return _items.TryDequeue(out item);
        }
        public T RentOrCreate() {
            return _items.TryDequeue(out var item) ? item : _instantiator.GetInstance();
        }

        public bool Return(T item) {
            if (_items.Count >= Capacity)
                return false;

            _resetItemFunc?.Invoke(item);
            _items.Enqueue(item);
            return true;

        }

        public int Return(IEnumerable<T> items) {
            return items.Count(Return);
        }
    }
}
