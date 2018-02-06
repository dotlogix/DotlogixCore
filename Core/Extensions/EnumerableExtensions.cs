// ==================================================
// Copyright 2016(C) , DotLogix
// File:  EnumerableExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Extensions {
    public static class EnumerableExtensions {
        public static DiffCollection<T> Diff<T>(this IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer = null) {
            return new DiffCollection<T>(left, right, comparer);
        }

        public static DiffCollection<T> Diff<T, TKey>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, TKey> keySelector) where TKey : IComparable {
            return new DiffCollection<T>(left, right, new SelectorEqualityComparer<T,TKey>(keySelector));
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable, bool emptyIfNull = true) {
            if(enumerable == null)
                return emptyIfNull ? new HashSet<T>() : null;
            return new HashSet<T>(enumerable);
        }

        public static T[] TakeLast<T>(this IEnumerable<T> enumerable, int count) {
            return enumerable.ToList().TakeLast(count);
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
            var list = enumerable.ToList();
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
                    } else {
                        enumerator.Dispose();
                    }
                }
            } finally {
                if(queue != null)
                    foreach(var enumerator in queue) {
                        enumerator.Dispose();
                    }
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> enumerables) {
            foreach(var enumerable in enumerables) {
                foreach(var value in enumerable) {
                    yield return value;
                }
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<IEnumerable<T>> enumerables) {
            var list = new List<T>();
            foreach(var enumerable in enumerables) {
                list.AddRange(enumerable);
            }
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
    }
}
