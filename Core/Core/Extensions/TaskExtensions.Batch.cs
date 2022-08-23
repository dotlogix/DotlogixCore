// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  TaskExtensions2.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.11.2021 20:38
// LastEdited:  09.11.2021 20:38
// ==================================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.Core.Extensions; 

[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
public static partial class TaskExtensions {
    public const int InfiniteDegreeOfParallelism = -1;

    public static async Task<ICollection<TTarget>> TransformAsync<TSource, TTarget>(
        this IEnumerable<TSource> enumerable,
        Func<TSource, Task<TTarget>> selector,
        int maxDegreeOfParallelism = InfiniteDegreeOfParallelism,
        CancellationToken cancellationToken = default
    )
    {
        if(maxDegreeOfParallelism < 1) {
            maxDegreeOfParallelism = int.MaxValue;
        }

        using var concurrency = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
        return await Task.WhenAll(enumerable.Select(GetResultAsync));
            
        async Task<TTarget> GetResultAsync(TSource source) {
            try {
                await concurrency.WaitAsync(cancellationToken);
                return await selector.Invoke(source);
            } finally {
                concurrency.Release();
            }
        }
    }

    public static async Task<ICollection<(TSource source, TTarget target)>> BatchAsync<TSource, TTarget>(
        this IEnumerable<TSource> enumerable,
        Func<TSource, Task<TTarget>> callback,
        int maxDegreeOfParallelism = InfiniteDegreeOfParallelism,
        CancellationToken cancellationToken = default
    ) {
        if(maxDegreeOfParallelism < 1) {
            maxDegreeOfParallelism = int.MaxValue;
        }

        using var concurrency = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
        return await Task.WhenAll(enumerable.Select(GetResultAsync));
            
        async Task<(TSource source, TTarget target)> GetResultAsync(TSource source) {
            try {
                await concurrency.WaitAsync(cancellationToken);
                var target = await callback.Invoke(source);
                return (source, target);
            } finally {
                concurrency.Release();
            }
        }
    }

    public static async Task BatchAsync<TSource>(
        this IEnumerable<TSource> enumerable,
        Func<TSource, Task> callback,
        int maxDegreeOfParallelism = InfiniteDegreeOfParallelism,
        CancellationToken cancellationToken = default
    ) {
        if(maxDegreeOfParallelism < 1) {
            maxDegreeOfParallelism = int.MaxValue;
        }

        using var concurrency = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
        await Task.WhenAll(enumerable.Select(GetResultAsync));
            
        async Task GetResultAsync(TSource source) {
            try {
                await concurrency.WaitAsync(cancellationToken);
                await callback.Invoke(source);
            } finally {
                concurrency.Release();
            }
        }
    }
        
    public static async Task<ICollection<TTarget>> TransformAsync<TSource, TTarget>(
        this IEnumerable<TSource> enumerable,
        Func<TSource, ValueTask<TTarget>> selector,
        int maxDegreeOfParallelism = InfiniteDegreeOfParallelism,
        CancellationToken cancellationToken = default
    )
    {
        if(maxDegreeOfParallelism < 1) {
            maxDegreeOfParallelism = int.MaxValue;
        }

        using var concurrency = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
        return await Task.WhenAll(enumerable.Select(GetResultAsync));
            
        async Task<TTarget> GetResultAsync(TSource source) {
            try {
                await concurrency.WaitAsync(cancellationToken);
                return await selector.Invoke(source);
            } finally {
                concurrency.Release();
            }
        }
    }

    public static async Task<ICollection<(TSource source, TTarget target)>> BatchAsync<TSource, TTarget>(
        this IEnumerable<TSource> enumerable,
        Func<TSource, ValueTask<TTarget>> callback,
        int maxDegreeOfParallelism = InfiniteDegreeOfParallelism,
        CancellationToken cancellationToken = default
    ) {
        if(maxDegreeOfParallelism < 1) {
            maxDegreeOfParallelism = int.MaxValue;
        }

        using var concurrency = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
        return await Task.WhenAll(enumerable.Select(GetResultAsync));
            
        async Task<(TSource source, TTarget target)> GetResultAsync(TSource source) {
            try {
                await concurrency.WaitAsync(cancellationToken);
                var target = await callback.Invoke(source);
                return (source, target);
            } finally {
                concurrency.Release();
            }
        }
    }

    public static async Task BatchAsync<TSource>(
        this IEnumerable<TSource> enumerable,
        Func<TSource, ValueTask> callback,
        int maxDegreeOfParallelism = InfiniteDegreeOfParallelism,
        CancellationToken cancellationToken = default
    ) {
        if(maxDegreeOfParallelism < 1) {
            maxDegreeOfParallelism = int.MaxValue;
        }

        using var concurrency = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
        var tasks = enumerable
           .Select(GetResultAsync)
           .Where(t => t.IsCompleted == false)
           .ToList();
        if(tasks.Count > 0) {
            await Task.WhenAll(tasks);
        }
            
        async Task GetResultAsync(TSource source) {
            try {
                await concurrency.WaitAsync(cancellationToken);
                await callback.Invoke(source);
            } finally {
                concurrency.Release();
            }
        }
    }
}