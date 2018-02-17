// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ListExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Extensions {
    public class SelectorComparer<T, TKey> : IComparer<T> where TKey : IComparable
    {
        private readonly Func<T, TKey> _comparableSelector;

        public SelectorComparer(Func<T, TKey> comparableSelector)
        {
            this._comparableSelector = comparableSelector;
        }

        public int Compare(T x, T y)
        {
            return _comparableSelector.Invoke(x).CompareTo(_comparableSelector.Invoke(y));
        }
    }

    public class SelectorEqualityComparer<T, TKey> : IEqualityComparer<T> where TKey : IComparable
    {
        private readonly Func<T, TKey> _comparableSelector;

        public SelectorEqualityComparer(Func<T, TKey> comparableSelector)
        {
            this._comparableSelector = comparableSelector;
        }

        public int Compare(T x, T y)
        {
            return _comparableSelector(x).CompareTo(_comparableSelector(y));
        }

        public bool Equals(T x, T y) {
            
            return Equals(_comparableSelector(x), _comparableSelector(y));
        }

        public int GetHashCode(T x) {
            return _comparableSelector(x).GetHashCode();
        }
    }

    public static class ListExtensions {
        

        public static T[] TakeLast<T>(this IList<T> list, int count) {
            if(count < 0)
                throw new ArgumentException("Count has to be greater or equal zero", nameof(count));

            var listCount = list.Count;
            count = Math.Min(count, listCount);
            var elements = new T[count];
            var startIndex = listCount - count;
            for(var i = 0; i < count; i++) {
                elements[i] = list[startIndex + i];
            }
            return elements;
        }

        public static IList<T> Swap<T>(this IList<T> list, int index1, int index2) {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return list;
        }

        public static void InsertSorted<T>(this List<T> list, T item) where T : IComparable<T>
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return;
            }
            if (list[list.Count - 1].CompareTo(item) <= 0)
            {
                list.Add(item);
                return;
            }
            if (list[0].CompareTo(item) >= 0)
            {
                list.Insert(0, item);
                return;
            }
            int index = list.BinarySearch(item);
            if (index < 0)
                index = ~index;
            list.Insert(index, item);
        }

        public static void InsertSorted<T, TKey>(this List<T> list, T item, Func<T, TKey> comparableSelector) where TKey : IComparable {
            InsertSorted(list, item, new SelectorComparer<T, TKey>(comparableSelector));
        }

        public static void InsertSorted<T>(this List<T> list, T item, IComparer<T> comparer)
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return;
            }
            if (comparer.Compare(list[list.Count - 1], item) <= 0)
            {
                list.Add(item);
                return;
            }
            if (comparer.Compare(list[0], item) >= 0)
            {
                list.Insert(0, item);
                return;
            }
            int index = list.BinarySearch(item);
            if (index < 0)
                index = ~index;
            list.Insert(index, item);
        }
    }
}
