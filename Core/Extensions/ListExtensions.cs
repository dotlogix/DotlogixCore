// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ListExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Extensions {
    public class SelectorComparer<T, TKey> : IComparer<T> where TKey : IComparable {
        private readonly Func<T, TKey> _comparableSelector;

        public SelectorComparer(Func<T, TKey> comparableSelector) {
            _comparableSelector = comparableSelector;
        }

        public int Compare(T x, T y) {
            return _comparableSelector.Invoke(x).CompareTo(_comparableSelector.Invoke(y));
        }
    }

    public class SelectorEqualityComparer<T, TKey> : IEqualityComparer<T> where TKey : IComparable {
        private readonly Func<T, TKey> _comparableSelector;

        public SelectorEqualityComparer(Func<T, TKey> comparableSelector) {
            _comparableSelector = comparableSelector;
        }

        public bool Equals(T x, T y) {
            return Equals(_comparableSelector(x), _comparableSelector(y));
        }

        public int GetHashCode(T x) {
            return _comparableSelector(x).GetHashCode();
        }

        public int Compare(T x, T y) {
            return _comparableSelector(x).CompareTo(_comparableSelector(y));
        }
    }

    public static class ListExtensions {
        /// <summary>
        /// Takes the last n elements of a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list</param>
        /// <param name="count">The maximum elemnts to take</param>
        /// <returns></returns>
        public static T[] TakeLast<T>(this IList<T> list, int count) {
            if(count < 0)
                throw new ArgumentException("Count has to be greater or equal zero", nameof(count));

            var listCount = list.Count;
            count = Math.Min(count, listCount);
            var elements = new T[count];
            var startIndex = listCount - count;
            for(var i = 0; i < count; i++)
                elements[i] = list[startIndex + i];
            return elements;
        }

        /// <summary>
        /// Swaps two values of a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list</param>
        /// <param name="index1">The index of the first element</param>
        /// <param name="index2">The index of the second element</param>
        /// <returns></returns>
        public static IList<T> Swap<T>(this IList<T> list, int index1, int index2) {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return list;
        }

        /// <summary>
        /// Inserts a value in a sorted order. Only works in lists where the values are sorted already
        /// </summary>
        /// <param name="list">The list</param>
        /// <param name="item">The item to insert</param>
        public static void InsertSorted<T>(this List<T> list, T item) where T : IComparable<T> {
            if(list.Count == 0) {
                list.Add(item);
                return;
            }
            if(list[list.Count - 1].CompareTo(item) <= 0) {
                list.Add(item);
                return;
            }
            if(list[0].CompareTo(item) >= 0) {
                list.Insert(0, item);
                return;
            }
            var index = list.BinarySearch(item);
            if(index < 0)
                index = ~index;
            list.Insert(index, item);
        }

        /// <summary>
        /// Inserts a value in a sorted order. Only works in lists where the values are sorted already
        /// </summary>
        /// <param name="list">The list</param>
        /// <param name="item">The item to insert</param>
        /// <param name="comparableSelector">The fucntion to select the key for sorting</param>
        public static void InsertSorted<T, TKey>(this List<T> list, T item, Func<T, TKey> comparableSelector) where TKey : IComparable {
            InsertSorted(list, item, new SelectorComparer<T, TKey>(comparableSelector));
        }

        /// <summary>
        /// Inserts a value in a sorted order. Only works in lists where the values are sorted already
        /// </summary>
        /// <param name="list">The list</param>
        /// <param name="item">The item to insert</param>
        /// <param name="comparer">The comparer used for sorting</param>
        public static void InsertSorted<T>(this List<T> list, T item, IComparer<T> comparer) {
            if(list.Count == 0) {
                list.Add(item);
                return;
            }
            if(comparer.Compare(list[list.Count - 1], item) <= 0) {
                list.Add(item);
                return;
            }
            if(comparer.Compare(list[0], item) >= 0) {
                list.Insert(0, item);
                return;
            }
            var index = list.BinarySearch(item);
            if(index < 0)
                index = ~index;
            list.Insert(index, item);
        }
    }
}
