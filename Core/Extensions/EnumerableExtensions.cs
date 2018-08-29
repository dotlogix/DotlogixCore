// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EnumerableExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Collections;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Extensions {
    public static class EnumerableExtensions {
        /// <summary>
        ///     Creates an enumerable of items using a selectorFunc
        /// </summary>
        /// <param name="initialValue">The initial value</param>
        /// <param name="selectNextFunc">A function to select the next item</param>
        /// <param name="hasNextFunc">A function to check if an additional value is available</param>
        /// <param name="yieldInitial">A flag if the initial value should be yield or skipped</param>
        /// <returns></returns>
        public static IEnumerable<T> Enumerate<T>(this T initialValue, Func<T, T> selectNextFunc, Func<T, bool> hasNextFunc, bool yieldInitial = false) {
            if(yieldInitial)
                yield return initialValue;
            while(hasNextFunc(initialValue))
                yield return initialValue = selectNextFunc(initialValue);
        }

        /// <summary>
        ///     Creates an enumerable of items using a selectorFunc
        /// </summary>
        /// <param name="initialValue">The initial value</param>
        /// <param name="selectNextFunc">A function to select the next item</param>
        /// <param name="conditionFunc">A function to check if the current value should be yield</param>
        /// <param name="yieldInitial">A flag if the initial value should be yield or skipped</param>
        /// <returns></returns>
        public static IEnumerable<T> EnumerateUntil<T>(this T initialValue, Func<T, T> selectNextFunc, Func<T, bool> conditionFunc, bool yieldInitial = false) {
            if(yieldInitial)
                yield return initialValue;

            initialValue = selectNextFunc(initialValue);
            while(conditionFunc(initialValue)) {
                yield return initialValue;
                initialValue = selectNextFunc(initialValue);
            }
        }

        /// <summary>
        ///     Initializes every element of the <see cref="T:System.Array"></see> with the provided value.
        /// </summary>
        /// <param name="array">The array</param>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public static T[] Initialize<T>(this T[] array, T value) {
            for(var i = 0; i < array.Length; i++)
                array[i] = value;
            return array;
        }

        /// <summary>
        ///     Creates a <see cref="Array"></see> by repeating the value n times
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="count">The amount of elements in the array</param>
        /// <returns></returns>
        public static T[] CreateArray<T>(this T value, int count = 1) {
            return Initialize(new T[count], value);
        }

        /// <summary>
        ///     Creates a <see cref="List{T}"></see> by repeating the value n times
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="count">The amount of elements in the list</param>
        /// <returns></returns>
        public static List<T> CreateList<T>(this T value, int count = 1) {
            return CreateArray(value, count).ToList();
        }

        /// <summary>
        ///     Creates a <see cref="IEnumerable{T}" /> by repeating the value n times
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="count">The amount of elements in the list</param>
        /// <returns></returns>
        public static IEnumerable<T> CreateEnumerable<T>(this T value, int count = 1) {
            for(var i = 0; i < count; i++)
                yield return value;
        }

        /// <summary>
        ///     Searches for differences of two enumerables using an equality comparer
        /// </summary>
        /// <param name="left">The first enumerable</param>
        /// <param name="right">The second enumerable</param>
        /// <param name="comparer">The comparer used to check equality</param>
        /// <returns></returns>
        public static DiffEnumerable<T> Diff<T>(this IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer = null) {
            if(comparer == null)
                comparer = EqualityComparer<T>.Default;

            var leftOnly = new HashSet<T>(left, comparer);
            var rightOnly = new HashSet<T>(right, comparer);
            var intersect = new HashSet<T>(leftOnly, comparer);

            intersect.IntersectWith(rightOnly);

            if(intersect.Count > 0) {
                leftOnly.ExceptWith(intersect);
                rightOnly.ExceptWith(intersect);
            }

            return new DiffEnumerable<T>(leftOnly, intersect, rightOnly);
        }

        /// <summary>
        ///     Searches for differences of two enumerables using a common key and a comparer
        /// </summary>
        /// <param name="left">The first enumerable</param>
        /// <param name="right">The second enumerable</param>
        /// <param name="keySelector">The function to select the key to check equality</param>
        /// <param name="comparer">The comparer used to check equality</param>
        /// <returns></returns>
        public static DiffEnumerable<T, T> Diff<T, TKey>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer = null) where TKey : IComparable {
            return left.Diff(keySelector, right, keySelector, comparer);
        }

        /// <summary>
        ///     Searches for differences of two enumerables using a common key and a comparer
        /// </summary>
        /// <param name="left">The first enumerable</param>
        /// <param name="leftKeySelector">The function to select the key to check equality</param>
        /// <param name="right">The second enumerable</param>
        /// <param name="rightKeySelector">The function to select the key to check equality</param>
        /// <param name="comparer">The comparer used to check equality</param>
        /// <returns></returns>
        public static DiffEnumerable<TLeft, TRight> Diff<TLeft, TRight, TKey>(this IEnumerable<TLeft> left, Func<TLeft, TKey> leftKeySelector, IEnumerable<TRight> right, Func<TRight, TKey> rightKeySelector, IEqualityComparer<TKey> comparer = null) where TKey : IComparable {
            var leftDictionary = left.ToDictionary(leftKeySelector);
            var rightDictionary = right.ToDictionary(rightKeySelector);

            var keyDiff = leftDictionary.Keys.Diff(rightDictionary.Keys, comparer);

            var leftOnly = keyDiff.LeftOnly.Select(k => leftDictionary[k]).ToList();
            var rightOnly = keyDiff.RightOnly.Select(k => rightDictionary[k]).ToList();
            var intersection = keyDiff.Intersect.Select(k => new DiffEnumerable<TLeft, TRight>.DiffValue(leftDictionary[k], rightDictionary[k])).ToList();

            return new DiffEnumerable<TLeft, TRight>(leftOnly, intersection, rightOnly);
        }

        /// <summary>
        ///     Converts a enumerable to a hashset
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable) {
            return new HashSet<T>(enumerable);
        }

        /// <summary>
        ///     Converts a enumerable to a hashset
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <param name="comparer">The comparer</param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer) {
            return new HashSet<T>(enumerable, comparer);
        }

        /// <summary>
        ///     Converts a enumerable to a list, but checks if it is already a list
        /// </summary>
        public static List<T> AsList<T>(this IEnumerable<T> enumerable) {
            if(enumerable is List<T> list)
                return list;

            return new List<T>(enumerable);
        }

        /// <summary>
        ///     Converts a enumerable to a collection, but checks if it is already a collection
        /// </summary>
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> enumerable) {
            if(enumerable is ICollection<T> collection)
                return collection;

            return new List<T>(enumerable);
        }

        /// <summary>
        ///     Converts a enumerable to an array, but checks if it is already an array
        /// </summary>
        public static T[] AsArray<T>(this IEnumerable<T> enumerable) {
            if(enumerable is T[] array)
                return array;
            return enumerable.ToArray();
        }

        /// <summary>
        ///     Takes the last n elements of a enumerable.
        /// </summary>
        public static T[] TakeLast<T>(this IEnumerable<T> enumerable, int count) {
            return enumerable.AsList().TakeLast(count);
        }

        /// <summary>
        ///     Takes the n random elements of a enumerable.
        /// </summary>
        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count) {
            return source.Shuffle().Take(count);
        }

        /// <summary>
        ///     Shuffles the elements in a enumerable
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable) {
            return Shuffle(enumerable, new Random());
        }

        /// <summary>
        ///     Shuffles the elements in a enumerable
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, int seed) {
            return Shuffle(enumerable, new Random(seed));
        }

        /// <summary>
        ///     Shuffles the elements in a enumerable
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, Random random) {
            var list = enumerable.AsList();
            var i = list.Count;
            do {
                var rand = random.Next(i);
                list.Swap(rand, i);
                yield return list[--i];
            } while(i > 1);
            yield return list[0];
        }

        /// <summary>
        ///     Returns the round robin mixed combination of a list of enumerables
        /// </summary>
        public static IEnumerable<T> Balance<T>(this IEnumerable<IEnumerable<T>> enumerables) {
            Queue<IEnumerator<T>> queue = null;
            try {
                queue = new Queue<IEnumerator<T>>(enumerables.Select(enumerable => enumerable.GetEnumerator()));
                while(queue.Count > 0) {
                    var enumerator = queue.Dequeue();
                    if(enumerator.MoveNext()) {
                        yield return enumerator.Current;
                        queue.Enqueue(enumerator);
                    } else
                        enumerator.Dispose();
                }
            } finally {
                if(queue != null) {
                    foreach(var enumerator in queue)
                        enumerator.Dispose();
                }
            }
        }

        /// <summary>
        ///     Returns the combination of a list of enumerables
        /// </summary>
        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> enumerables) {
            foreach(var enumerable in enumerables) {
                foreach(var value in enumerable)
                    yield return value;
            }
        }

        /// <summary>
        ///     Returns the shuffled combination of a list of enumerables
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<IEnumerable<T>> enumerables) {
            var list = new List<T>();
            foreach(var enumerable in enumerables)
                list.AddRange(enumerable);
            return list.Shuffle();
        }

        /// <summary>
        ///     Combines a list of enumerable using the given combination mode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerables"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IEnumerable<T> Combine<T>(this IEnumerable<IEnumerable<T>> enumerables, CombineMode mode) {
            switch(mode) {
                case CombineMode.Sequential:
                    return enumerables.Concat();
                case CombineMode.RoundRobin:
                    return enumerables.Balance();
                case CombineMode.Shuffled:
                    return enumerables.Shuffle();
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>
        ///     Combines a list of enumerable using the given combination mode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="otherEnumerable"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IEnumerable<T> Combine<T>(this IEnumerable<T> enumerable, IEnumerable<T> otherEnumerable, CombineMode mode) {
            switch(mode) {
                case CombineMode.Sequential:
                    return enumerable.Concat(otherEnumerable);
                case CombineMode.RoundRobin:
                    return Balance(new[] {enumerable, otherEnumerable});
                case CombineMode.Shuffled:
                    return Shuffle(new[] {enumerable, otherEnumerable});
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        /// <summary>
        ///     Combines a list of enumerable using the given combination mode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerables"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IEnumerable<T> Combine<T>(CombineMode mode, params IEnumerable<T>[] enumerables) {
            return Combine(enumerables, mode);
        }

        /// <summary>
        ///     Takes chunks of elements from a enumerable
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <param name="chunkSize">The size of the chunked parts</param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Chunk<T>(this IEnumerable<T> enumerable, int chunkSize) {
            var currentCount = 1;
            var list = new List<T>(chunkSize);
            foreach(var item in enumerable) {
                list.Add(item);
                if(currentCount < chunkSize) {
                    currentCount++;
                    continue;
                }

                yield return list;
                currentCount = 1;
                list = new List<T>(chunkSize);
            }
            if(list.Count > 0)
                yield return list;
        }

        /// <summary>
        ///     Creates a hierarchy of elements using a key to determ inheritance
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="items">The items</param>
        /// <param name="keySelector">The function to get the key of the current element</param>
        /// <param name="parentKeySelector">The function to get the key of the parent element</param>
        /// <returns></returns>
        public static Hierarchy<TKey, TValue> ToHierarchy<TKey, TValue>(this IEnumerable<TValue> items, Func<TValue, TKey> keySelector, Func<TValue, TKey> parentKeySelector) {
            var root = new Hierarchy<TKey, TValue>(default);
            var dict = new Dictionary<TKey, Hierarchy<TKey, TValue>>();
            var grouped = items.GroupBy(parentKeySelector.Invoke).ToList();
            while(grouped.Count > 0) {
                var prevCount = grouped.Count;
                for(var i = prevCount - 1; i >= 0; i--) {
                    var group = grouped[i];
                    var groupKey = group.Key;
                    Hierarchy<TKey, TValue> parent;
                    if(Equals(groupKey, default(TKey)))
                        parent = root;
                    else if(dict.TryGetValue(groupKey, out parent) == false)
                        continue;

                    grouped.RemoveAt(i);

                    foreach(var value in group) {
                        var hierarchy = parent.Add(keySelector.Invoke(value), value);
                        dict.Add(hierarchy.Key, hierarchy);
                    }
                }

                if(grouped.Count == prevCount)
                    throw new InvalidOperationException("Can not resolve the whole hierarchy, maxbe there are some cross dependencies");
            }
            return root;
        }
    }
}
