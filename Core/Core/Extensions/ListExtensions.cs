// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ListExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  15.08.2018
// LastEdited:  20.11.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="List{T}"/>
    /// </summary>
    public static class ListExtensions {
        /// <summary>
        ///     Takes the last n elements of a list
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
        ///     Swaps two values of a list
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
        ///     Inserts a value in a sorted order. Only works in lists where the values are sorted already
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
        ///     Inserts a value in a sorted order. Only works in lists where the values are sorted already
        /// </summary>
        /// <param name="list">The list</param>
        /// <param name="item">The item to insert</param>
        /// <param name="comparableSelector">The fucntion to select the key for sorting</param>
        public static void InsertSorted<T, TKey>(this List<T> list, T item, Func<T, TKey> comparableSelector) where TKey : IComparable {
            InsertSorted(list, item, new SelectorComparer<T, TKey>(comparableSelector));
        }

        /// <summary>
        ///     Inserts a value in a sorted order. Only works in lists where the values are sorted already
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

            var index = list.BinarySearch(item, comparer);
            if(index < 0)
                index = ~index;
            list.Insert(index, item);
        }


        /// <summary>
        ///     Adds the elements of the specified collection to the end of the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="list">The list</param>
        /// <param name="collection">
        ///     The collection whose elements should be added to the end of the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />. The collection itself cannot be <see langword="null" />,
        ///     but it can contain elements that are <see langword="null" />, if type <typeparam name="T" /> is a reference type.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection" /> is <see langword="null" />.
        /// </exception>
        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> collection) {
            if(list is List<T> realList) {
                realList.AddRange(collection);
                return;
            }

            foreach(var value in collection)
                list.Add(value);
        }

        /// <summary>
        ///     Adds the elements of the specified collection to the end of the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="list">The list</param>
        /// <param name="collection">
        ///     The collection whose elements should be added to the end of the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />. The collection itself cannot be <see langword="null" />,
        ///     but it can contain elements that are <see langword="null" />, if type <typeparam name="T" /> is a reference type.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection" /> is <see langword="null" />.
        /// </exception>
        public static void RemoveRange<T>(this ICollection<T> list, IEnumerable<T> collection) {
            foreach(var value in collection)
                list.Remove(value);
        }

        /// <summary>
        ///     Creates a <see cref="List{T}"></see> by repeating the value n times
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="count">The amount of elements in the list</param>
        /// <returns></returns>
        public static List<T> CreateList<T>(this T value, int count = 1) {
            return value.CreateArray(count).ToList();
        }
        
        /// <summary>
        ///     Creates a <see cref="IEnumerable{T}" /> by repeating the value n times
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="count">The amount of elements in the list</param>
        /// <returns></returns>
        public static Task<List<T>> CreateList<T>(this Task<T> value, int count = 1) {
            return value.TransformAsync(v => v.Result.CreateList(count));
        }
    }
}