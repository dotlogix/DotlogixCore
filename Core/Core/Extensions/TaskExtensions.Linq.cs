// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  TaskExtensions2.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.11.2021 20:38
// LastEdited:  09.11.2021 20:38
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Extensions; 

public static partial class TaskExtensions {
    public static async Task<Task> WhenAny(this IEnumerable<Task> tasks) {
        return await Task.WhenAny(tasks);
    }
        
    public static async Task<Task<T>> WhenAny<T>(this IEnumerable<Task<T>> tasks) {
        return await Task.WhenAny(tasks);
    }

    public static Task WhenAll(this IEnumerable<Task> tasks) {
        return Task.WhenAll(tasks);
    }
        
    public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks) {
        return Task.WhenAll(tasks);
    }

    public static ValueTask WhenAll(this IEnumerable<ValueTask> valueTasks) {
        var tasks = valueTasks.Where(t => !t.IsCompletedSuccessfully).Select(t => t.AsTask()).ToList();
        return tasks.Count == 0 ? ValueTask.CompletedTask : new ValueTask(Task.WhenAll(tasks));
    }
        
    public static async ValueTask<T[]> WhenAll<T>(this IEnumerable<ValueTask<T>> tasks) {
        var taskList = tasks.AsReadOnlyList();
        var results = new T[taskList.Count];
        for(var i = 0; i < taskList.Count; i++) {
            results[i] = await taskList[i];
        }
        return results;
    }
        
    public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<Task<T>> tasks) {
        return new TaskEnumerable<T>(tasks);
    }
        
    public static Task<IEnumerable<TSource>> ToEnumerableAsync<TSource>(this IEnumerable<Task<TSource>> tasks) {
        return Task.WhenAll(tasks).TransformAsync(Enumerable.AsEnumerable);
    }

    public static Task<ICollection<TSource>> ToCollectionAsync<TSource>(this IEnumerable<Task<TSource>> tasks) {
        return Task.WhenAll(tasks).TransformAsync(EnumerableExtensions.ToCollection);
    }

    public static Task<IReadOnlyCollection<TSource>> ToReadOnlyCollectionAsync<TSource>(this IEnumerable<Task<TSource>> tasks) {
        return Task.WhenAll(tasks).TransformAsync(EnumerableExtensions.ToReadOnlyCollection);
    }

    public static Task<List<TSource>> ToListAsync<TSource>(this IEnumerable<Task<TSource>> tasks) {
        return Task.WhenAll(tasks).TransformAsync(Enumerable.ToList);
    }

    public static Task<IReadOnlyList<TSource>> ToReadOnlyListAsync<TSource>(this IEnumerable<Task<TSource>> tasks) {
        return Task.WhenAll(tasks).TransformAsync(EnumerableExtensions.ToReadOnlyList);
    }

    public static Task<TSource[]> ToArrayAsync<TSource>(this IEnumerable<Task<TSource>> tasks) {
        return Task.WhenAll(tasks);
    }

    public static Task<HashSet<TSource>> ToHashSetAsync<TSource>(this IEnumerable<Task<TSource>> tasks) {
        return Task.WhenAll(tasks).TransformAsync(Enumerable.ToHashSet);
    }
        
    public static Task<HashSet<TSource>> ToHashSetAsync<TSource>(this IEnumerable<Task<TSource>> tasks, IEqualityComparer<TSource> comparer) {
        return Task.WhenAll(tasks).TransformAsync(results => results.ToHashSet(comparer));
    }
        
    public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IEnumerable<Task<TSource>> tasks, Func<TSource, TKey> keySelector) {
        return Task.WhenAll(tasks).TransformAsync(results => results.ToDictionary(keySelector));
    }

    public static Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(this IEnumerable<Task<TSource>> tasks, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector) {
        return Task.WhenAll(tasks).TransformAsync(results => results.ToDictionary(keySelector, valueSelector));
    }

    public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IEnumerable<Task<TSource>> tasks, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) {
        return Task.WhenAll(tasks).TransformAsync(results => results.ToDictionary(keySelector, comparer));
    }

    public static Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(this IEnumerable<Task<TSource>> tasks, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey> comparer) {
        return Task.WhenAll(tasks).TransformAsync(results => results.ToDictionary(keySelector, valueSelector, comparer));
    }

    private class TaskEnumerable<T> : IAsyncEnumerable<T> {
        private readonly IEnumerable<Task<T>> _tasks;

        public TaskEnumerable(IEnumerable<Task<T>> tasks) {
            _tasks = tasks;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) {
            return new TaskEnumerator<T>(_tasks.GetEnumerator(), cancellationToken);
        }
    }
        
    private class TaskEnumerator<T> : IAsyncEnumerator<T> {
        private readonly IEnumerator<Task<T>> _taskEnumerator;
        private readonly CancellationToken _cancellationToken;

        public TaskEnumerator(IEnumerator<Task<T>> taskEnumerator, CancellationToken cancellationToken = default) {
            _taskEnumerator = taskEnumerator;
            _cancellationToken = cancellationToken;
        }

        public T Current { get; private set; }

        public ValueTask<bool> MoveNextAsync() {
            _cancellationToken.ThrowIfCancellationRequested();
                
            Task<T> current;
            do {
                if(_taskEnumerator.MoveNext() == false) {
                    return new ValueTask<bool>(false);
                }
                current = _taskEnumerator.Current;
            } while(current == null);

            if(current.IsCompleted || current.IsFaulted) {
                Current = current.GetAwaiter().GetResult();
                return new ValueTask<bool>(true);
            }

            var moveNextTask = current.TransformAsync(t => { Current = t.Result; return true; });
            return new ValueTask<bool>(moveNextTask);
        }

        public ValueTask DisposeAsync() {
            _taskEnumerator.Dispose();
            return new ValueTask();
        }
    }
}