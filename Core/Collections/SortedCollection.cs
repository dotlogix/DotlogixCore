// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SortedCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  22.12.2017
// LastEdited:  07.01.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Collections {
    public class SortedCollection<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        private readonly IComparer<T> _comparer;
        private readonly List<T> _itemList = new List<T>();

        public T this[int index] => _itemList[index];

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
            return _itemList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(T item) {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            _itemList.InsertSorted(item, _comparer);
        }

        public void Clear() {
            _itemList.Clear();
        }

        public bool Contains(T item) {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            return _itemList.BinarySearch(item, _comparer) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _itemList.CopyTo(array);
        }

        public bool Remove(T item) {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            var index = _itemList.BinarySearch(item);
            if(index >= 0) {
                _itemList.RemoveAt(index);
                return true;
            }
            return false;
        }

        public int Count => _itemList.Count;

        bool ICollection<T>.IsReadOnly => ((ICollection<T>)_itemList).IsReadOnly;
    }
}
