// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TaskExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using DotLogix.Core.Reflection.Delegates;
using DotLogix.Core.Reflection.Fluent;
#endregion

namespace DotLogix.Core.Extensions; 

/// <summary>
///     A static class providing extension methods for <see cref="Task" />
/// </summary>
public static partial class TaskExtensions {
    private static readonly ConcurrentDictionary<Type, GetterDelegate> TypeResultAccessors = new();
        
    static TaskExtensions() {
        var voidType = Type.GetType("System.Threading.Tasks.VoidTaskResult");
        var taskOfVoid = typeof(Task<>).MakeGenericType(voidType);
        TypeResultAccessors.TryAdd(taskOfVoid, null);
        TypeResultAccessors.TryAdd(typeof(Task), null);
    }

    /// <summary>
    /// A helper method to handle task in background and attach hooks as an option
    /// </summary>
    public static void RunInBackground(this Task task, Action<Task> onSuccess = null, Action<Task> onError = null, Action<Task> onCancelled = null) {
        if(onSuccess is not null)
            task.ContinueWith(onSuccess, TaskContinuationOptions.OnlyOnRanToCompletion);
        if(onError is not null)
            task.ContinueWith(onError, TaskContinuationOptions.OnlyOnFaulted);
        if(onCancelled is not null)
            task.ContinueWith(onCancelled, TaskContinuationOptions.OnlyOnFaulted);
        task.ConfigureAwait(false);
    }
        
    /// <summary>
    /// A helper method to handle task in background and attach hooks as an option
    /// </summary>
    public static void RunInBackground<T>(this Task<T> task, Action<Task<T>> onSuccess = null, Action<Task<T>> onError = null, Action<Task<T>> onCancelled = null) {
        if(onSuccess is not null)
            task.ContinueWith(onSuccess, TaskContinuationOptions.OnlyOnRanToCompletion);
        if(onError is not null)
            task.ContinueWith(onError, TaskContinuationOptions.OnlyOnFaulted);
        if(onCancelled is not null)
            task.ContinueWith(onCancelled, TaskContinuationOptions.OnlyOnFaulted);
        task.ConfigureAwait(false);
    }

    /// <summary>
    /// A helper method to handle task in background and attach hooks as an option
    /// </summary>
    public static void RunInBackground(this ValueTask valueTask, Action<ValueTask> onSuccess = null, Action<ValueTask> onError = null, Action<ValueTask> onCancelled = null) {
        if(valueTask.IsCompletedSuccessfully) {
            onSuccess?.Invoke(valueTask);
        }

        var task = valueTask.AsTask();
        if(onSuccess is not null)
            task.ContinueWith(t => onSuccess.Invoke(new ValueTask(t)), TaskContinuationOptions.OnlyOnRanToCompletion);
        if(onError is not null)
            task.ContinueWith(t => onError.Invoke(new ValueTask(t)), TaskContinuationOptions.OnlyOnFaulted);
        if(onCancelled is not null)
            task.ContinueWith(t => onCancelled.Invoke(new ValueTask(t)), TaskContinuationOptions.OnlyOnFaulted);
        task.ConfigureAwait(false);
    }
        
    /// <summary>
    /// A helper method to handle task in background and attach hooks as an option
    /// </summary>
    public static void RunInBackground<T>(this ValueTask<T> valueTask, Action<ValueTask<T>> onSuccess = null, Action<ValueTask<T>> onError = null, Action<ValueTask<T>> onCancelled = null) {
        if(valueTask.IsCompletedSuccessfully) {
            onSuccess?.Invoke(valueTask);
        }

        var task = valueTask.AsTask();
        if(onSuccess is not null)
            task.ContinueWith(t => onSuccess.Invoke(new ValueTask<T>(t)), TaskContinuationOptions.OnlyOnRanToCompletion);
        if(onError is not null)
            task.ContinueWith(t => onError.Invoke(new ValueTask<T>(t)), TaskContinuationOptions.OnlyOnFaulted);
        if(onCancelled is not null)
            task.ContinueWith(t => onCancelled.Invoke(new ValueTask<T>(t)), TaskContinuationOptions.OnlyOnFaulted);
        task.ConfigureAwait(false);
    }

        
    /// <summary>
    /// A helper method to call GetAwaiter().GetResult()
    /// </summary>
    public static void Block(Task task) {
        task.GetAwaiter().GetResult();
    }
        
    /// <summary>
    /// A helper method to call GetAwaiter().GetResult()
    /// </summary>
    public static T Block<T>(this Task<T> task) {
        return task.GetAwaiter().GetResult();
    }

        
    /// <summary>
    /// A helper method to call GetAwaiter().GetResult()
    /// </summary>
    public static void Block(this ValueTask task) {
        task.GetAwaiter().GetResult();
    }
        
    /// <summary>
    /// A helper method to call GetAwaiter().GetResult()
    /// </summary>
    public static T Block<T>(this ValueTask<T> task) {
        return task.GetAwaiter().GetResult();
    }

    /// <summary>
    ///     Takes a completed task and return its result as object. If the task does not have a value then null is returned.
    /// </summary>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static object Unpack(this Task task) {
        if(task.IsCompleted == false)
            throw new InvalidOperationException("Task has to be completed to unpack the result");

        var taskType = task.GetType();
        var accessor = TypeResultAccessors.GetOrAdd(taskType, CreateAccessor);
        return accessor?.Invoke(task);
    }

    /// <summary>
    ///     Takes a task and return its result as object. If the task does not have a value then null is returned
    /// </summary>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static Task<object> UnpackAsync(this Task task) {
        if (task is Task<object> objectTask)
            return objectTask;

        var tcs = new TaskCompletionSource<object>();

        task.ContinueWith(t => {
                if (t.IsCanceled) { 
                    tcs.SetCanceled();
                    return;
                }

                if (t.IsFaulted) {
                    if (t.Exception is not null)
                        tcs.SetException(t.Exception.InnerExceptions);
                    else
                        tcs.SetException(new ArgumentException("Exception must be present for a faulted task"));
                    return;
                }

                tcs.SetResult(Unpack(task));
            },
            TaskContinuationOptions.ExecuteSynchronously);
        return tcs.Task;
    }
        
    /// <summary>
    ///     Takes a task and return its result as object. If the task does not have a value then null is returned
    /// </summary>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static async Task WithAggregateException(this Task task) {
        try {
            await task.ConfigureAwait(false);
        } catch {
            if (task.Exception == null) throw;
            ExceptionDispatchInfo.Capture(task.Exception).Throw();
        }
    }
        
    /// <summary>
    ///     Takes a task and return its result as object. If the task does not have a value then null is returned
    /// </summary>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static async Task<TResult> WithAggregateException<TResult>(this Task<TResult> task) {
        try {
            return await task.ConfigureAwait(false);
        } catch {
            if (task.Exception == null) throw;
            ExceptionDispatchInfo.Capture(task.Exception).Throw();
            return default;
        }
    }
        
    /// <summary>
    ///     Converts the result of a task to a base type
    /// </summary>
    /// <typeparam name="TTo">The target type</typeparam>
    /// <typeparam name="TFrom">The current type</typeparam>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static Task<TTo> CastAsync<TFrom, TTo>(this Task<TFrom> task) {
        return task.TransformAsync(r => (TTo)(object)r.Result);
    }

    /// <summary>
    ///     Converts the result of a task to a base type
    /// </summary>
    /// <typeparam name="TTo">The target type</typeparam>
    /// <typeparam name="TFrom">The current type</typeparam>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static Task<TTo> AsAsync<TFrom, TTo>(this Task<TFrom> task) {
        return task.TransformAsync(r => r.Result is TTo to ? to : default);
    }

    /// <summary>
    ///     Converts the result of a task to a type using <see cref="ObjectExtension.ConvertTo{TTarget}"/>
    /// </summary>
    /// <typeparam name="TTo">The target type</typeparam>
    /// <typeparam name="TFrom">The current type</typeparam>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static Task<TTo> ConvertAsync<TFrom, TTo>(this Task<TFrom> task) {
        return task.TransformAsync(r => r.Result.ConvertTo<TTo>());
    }

    /// <summary>
    ///     Converts the result of a task using a selector method
    /// </summary>
    /// <typeparam name="TTo">The target type</typeparam>
    /// <typeparam name="TFrom">The current type</typeparam>
    /// <param name="onComplete">The function used to convert the result after completion</param>
    /// <param name="onError">The function used to convert the result after an error</param>
    /// <param name="onCancel">The function used to convert the result after cancellation</param>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static Task<TTo> TransformAsync<TFrom, TTo>(this Task<TFrom> task, Func<Task<TFrom>, TTo> onComplete, Func<Task<TFrom>, TTo> onError = null, Func<Task<TFrom>, TTo> onCancel = null) {
        var tcs = new TaskCompletionSource<TTo>();
        task.ContinueWith(t => {
                if(t.IsCanceled) {
                    if(onCancel is not null)
                        tcs.SetResult(onCancel.Invoke(t));
                    else
                        tcs.SetCanceled();
                    return;
                }

                if(t.IsFaulted) {
                    if(onError is not null)
                        tcs.SetResult(onError.Invoke(t));
                    else if(t.Exception is not null)
                        tcs.SetException(t.Exception.InnerExceptions);

                    return;
                }

                tcs.SetResult(onComplete.Invoke(t));
            },
            TaskContinuationOptions.ExecuteSynchronously);
        return tcs.Task;
    }

    /// <summary>
    ///     Converts the result of a task using a selector method
    /// </summary>
    /// <typeparam name="TTo">The target type</typeparam>
    /// <typeparam name="TFrom">The current type</typeparam>
    /// <param name="onComplete">The function used to convert the result after completion</param>
    /// <param name="onError">The function used to convert the result after an error</param>
    /// <param name="onCancel">The function used to convert the result after cancellation</param>
    /// <param name="task">The task</param>
    /// <returns></returns>
    public static Task<TTo> TransformAsync<TFrom, TTo>(this Task<TFrom> task, Func<TFrom, TTo> onComplete, Func<Exception, TTo> onError = null, Func<TTo> onCancel = null) {
        var tcs = new TaskCompletionSource<TTo>();
        task.ContinueWith(t => {
                if(t.IsCanceled) {
                    if(onCancel is not null)
                        tcs.SetResult(onCancel.Invoke());
                    else
                        tcs.SetCanceled();
                    return;
                }

                if(t.IsFaulted) {
                    if(onError is not null)
                        tcs.SetResult(onError.Invoke(t.Exception));
                    else if(t.Exception is not null)
                        tcs.SetException(t.Exception.InnerExceptions);

                    return;
                }

                tcs.SetResult(onComplete.Invoke(t.Result));
            },
            TaskContinuationOptions.ExecuteSynchronously);
        return tcs.Task;
    }
        
    private static GetterDelegate CreateAccessor(Type taskType) {
        var propertyInfo = taskType.GetProperty("Result");
        return propertyInfo is not null
            ? FluentIl.CreateGetter(propertyInfo)
            : null;
    }
}