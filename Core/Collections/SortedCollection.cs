// ==================================================
// Copyright 2019(C) , DotLogix
// File:  SortedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  07.02.2019
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Collections {
    public class SortedCollection<T> : ICollection<T>, IReadOnlyCollection<T> {
        private readonly IComparer<T> _comparer;
        private readonly List<T> _itemList = new List<T>();
        private readonly object _lock = new object();

        public T this[int index] {
            get {
                lock(_lock)
                    return _itemList[index];
            }
        }

        public SortedCollection() : this(Comparer<T>.Default) { }

        public SortedCollection(IEnumerable<T> items) : this(items, Comparer<T>.Default) { }

        public SortedCollection(IComparer<T> comparer) {
            _comparer = comparer;
        }

        public SortedCollection(IEnumerable<T> items, IComparer<T> comparer) {
            if(items == null)
                throw new ArgumentNullException(nameof(items));
            if(comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            _itemList.AddRange(items);
            _itemList.Sort(comparer);
        }

        public IEnumerator<T> GetEnumerator() {
            List<T> list;
            lock(_lock)
                list = _itemList;
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(T item) {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            lock(_lock)
                _itemList.InsertSorted(item, _comparer);
        }

        public void Clear() {
            lock(_lock)
                _itemList.Clear();
        }

        public bool Contains(T item) {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            lock(_lock)
                return _itemList.BinarySearch(item, _comparer) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            lock(_lock)
                _itemList.CopyTo(array);
        }

        public bool Remove(T item) {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            lock(_lock) {
                var index = _itemList.BinarySearch(item);
                if(index >= 0) {
                    _itemList.RemoveAt(index);
                    return true;
                }

                return false;
            }
        }

        public int Count {
            get {
                lock(_lock)
                    return _itemList.Count;
            }
        }

        bool ICollection<T>.IsReadOnly => false;

        public void AddRange(IEnumerable<T> items) {
            if(items == null)
                throw new ArgumentNullException(nameof(items));
            lock(_lock) {
                _itemList.AddRange(items);
                _itemList.Sort(_comparer);
            }
        }
    }
}
