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
    /// <summary>
    /// A sorted collection, duplicates are allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortedCollection<T> : ICollection<T>, IReadOnlyCollection<T> {
        private readonly IComparer<T> _comparer;
        private readonly List<T> _itemList = new List<T>();
        private readonly object _lock = new object();

        /// <summary>
        /// Get an item by index
        /// </summary>
        /// <param name="index"></param>
        public T this[int index] {
            get {
                lock(_lock)
                    return _itemList[index];
            }
        }

        /// <inheritdoc />
        public SortedCollection() : this(Comparer<T>.Default) { }

        /// <inheritdoc />
        public SortedCollection(IEnumerable<T> items) : this(items, Comparer<T>.Default) { }

        /// <inheritdoc />
        public SortedCollection(IComparer<T> comparer) {
            _comparer = comparer;
        }

        /// <inheritdoc />
        public SortedCollection(IEnumerable<T> items, IComparer<T> comparer) {
            if(items == null)
                throw new ArgumentNullException(nameof(items));
            if(comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            _itemList.AddRange(items);
            _itemList.Sort(comparer);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() {
            List<T> list;
            lock(_lock)
                list = _itemList;
            return list.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(T item) {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            lock(_lock)
                _itemList.InsertSorted(item, _comparer);
        }

        /// <inheritdoc />
        public void Clear() {
            lock(_lock)
                _itemList.Clear();
        }

        /// <inheritdoc />
        public bool Contains(T item) {
            if(item == null)
                throw new ArgumentNullException(nameof(item));
            lock(_lock)
                return _itemList.BinarySearch(item, _comparer) >= 0;
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex) {
            lock(_lock)
                _itemList.CopyTo(array);
        }

        /// <inheritdoc />
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

        /// <summary>
        /// The element count
        /// </summary>
        public int Count {
            get {
                lock(_lock)
                    return _itemList.Count;
            }
        }

        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// Add a range of items to the collection
        /// </summary>
        /// <param name="items"></param>
        /// <exception cref="ArgumentNullException"></exception>
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
