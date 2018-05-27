// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EnumerableExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
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
        public static DiffEnumerable<T> Diff<T>(this IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer = null) {
            if(comparer == null)
                comparer = EqualityComparer<T>.Default;

            var leftOnly = new HashSet<T>(left, comparer);
            var rightOnly = new HashSet<T>(right, comparer);
            var intersect = new HashSet<T>(leftOnly, comparer);
            
            intersect.ExceptWith(intersect);

            if(intersect.Count > 0) {
                leftOnly.ExceptWith(intersect);
                rightOnly.ExceptWith(intersect);
            }

            return new DiffEnumerable<T>(leftOnly, intersect, rightOnly);
        }

        public static DiffEnumerable<T, T> Diff<T, TKey>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer = null) where TKey : IComparable {
            return left.Diff(keySelector, right, keySelector, comparer);
        }

        public static DiffEnumerable<TLeft, TRight> Diff<TLeft, TRight, TKey>(this IEnumerable<TLeft> left, Func<TLeft, TKey> leftKeySelector, IEnumerable<TRight> right, Func<TRight, TKey> rightKeySelector, IEqualityComparer<TKey> comparer = null) where TKey : IComparable
        {
            var leftDictionary = left.ToDictionary(leftKeySelector);
            var rightDictionary = right.ToDictionary(rightKeySelector);

            var keyDiff = leftDictionary.Keys.Diff(rightDictionary.Keys, comparer);

            var leftOnly = keyDiff.LeftOnly.Select(k => leftDictionary[k]).ToList();
            var rightOnly = keyDiff.RightOnly.Select(k => rightDictionary[k]).ToList();
            var intersection = keyDiff.Intersect.Select(k => new DiffEnumerable<TLeft, TRight>.DiffValue(leftDictionary[k], rightDictionary[k])).ToList();

            return new DiffEnumerable<TLeft, TRight>(leftOnly, intersection, rightOnly);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable, bool emptyIfNull = true) {
            if(enumerable == null)
                return emptyIfNull ? new HashSet<T>() : null;
            return new HashSet<T>(enumerable);
        }

        public static List<T> AsList<T>(this IEnumerable<T> enumerable) {
            if(enumerable is List<T> list)
                return list;

            return new List<T>(enumerable);
        }

        public static T[] AsArray<T>(this IEnumerable<T> enumerable) {
            if(enumerable is T[] array)
                return array;
            return enumerable.ToArray();
        }

        public static T[] TakeLast<T>(this IEnumerable<T> enumerable, int count) {
            return enumerable.AsList().TakeLast(count);
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count) {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable) {
            return Shuffle(enumerable, new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, int seed) {
            return Shuffle(enumerable, new Random(seed));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, Random random) {
            var list = enumerable.AsList();
            var i = list.Count;
            do {
                var rand = random.Next(i);
                yield return list[--i];
                list.Swap(rand, i);
            } while(i > 1);
            yield return list[0];
        }


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

        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> enumerables) {
            foreach(var enumerable in enumerables) {
                foreach(var value in enumerable)
                    yield return value;
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<IEnumerable<T>> enumerables) {
            var list = new List<T>();
            foreach(var enumerable in enumerables)
                list.AddRange(enumerable);
            return list.Shuffle();
        }

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

        public static IEnumerable<T> Combine<T>(CombineMode mode, params IEnumerable<T>[] enumerables) {
            return Combine(enumerables, mode);
        }

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

        public static Hierarchy<TKey, TValue> ToHierarchy<TKey, TValue>(this IEnumerable<TValue> items, Func<TValue, TKey> keySelector, Func<TValue, TKey> parentKeySelector) {
            var root = new Hierarchy<TKey, TValue>(default(TKey));
            var dict = new Dictionary<TKey, Hierarchy<TKey, TValue>>();
            var grouped = items.GroupBy(parentKeySelector.Invoke).ToList();
            while(grouped.Count > 0) {
                var prevCount = grouped.Count;
                for(int i = prevCount-1; i >= 0; i--) {
                    var group = grouped[i];
                    var groupKey = group.Key;
                    Hierarchy<TKey, TValue> parent;
                    if(Equals(groupKey, default(TKey))) {
                        parent = root;
                    }
                    else if(dict.TryGetValue(groupKey, out parent) == false) {
                        continue;
                    }

                    grouped.RemoveAt(i);

                    foreach (var value in group) {
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
