﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TaskExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Core.Reflection.Delegates;
using DotLogix.Core.Reflection.Fluent;
#endregion

namespace DotLogix.Core.Extensions {
    /// <summary>
    /// A static class providing extension methods for <see cref="Task"/>
    /// </summary>
    public static class TaskExtensions {
        private static readonly ConcurrentDictionary<Type, GetterDelegate> TypeResultAccessors = new ConcurrentDictionary<Type, GetterDelegate>();

        static TaskExtensions() {
            var voidType = Type.GetType("System.Threading.Tasks.VoidTaskResult");
            var taskOfVoid = typeof(Task<>).MakeGenericType(voidType);
            TypeResultAccessors.TryAdd(taskOfVoid, null);
            TypeResultAccessors.TryAdd(typeof(Task), null);
        }

        /// <summary>
        ///     Takes a task and return its result as object. If the task does not have a value then null is returned
        /// </summary>
        /// <param name="task">The task</param>
        /// <returns></returns>
        public static Task<object> UnpackResultAsync(this Task task) {
            if(task is Task<object> objectTask)
                return objectTask;

            var tcs = new TaskCompletionSource<object>();

            task.ContinueWith(t => tcs.SetResult(UnpackResult(t)), TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
            // ReSharper disable once PossibleNullReferenceException
            task.ContinueWith(t => tcs.SetException(t.Exception.InnerExceptions), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            task.ContinueWith(t => tcs.SetCanceled(), TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.ExecuteSynchronously);
            return tcs.Task;
        }

        /// <summary>
        ///     Takes a completed task and return its result as object. If the task does not have a value then null is returned.
        /// </summary>
        /// <param name="task">The task</param>
        /// <returns></returns>
        public static object UnpackResult(this Task task) {
            if(task.IsCompleted == false)
                throw new InvalidOperationException("Task has to be completed to unpack the result");
            
            var taskType = task.GetType();
            var accessor = TypeResultAccessors.GetOrAdd(taskType, CreateAccessor);
            return accessor?.Invoke(task);
        }

        /// <summary>
        ///     Converts the result of a task to a base type
        /// </summary>
        /// <typeparam name="TBase">The target type</typeparam>
        /// <typeparam name="TDerived">The current type</typeparam>
        /// <param name="task">The task</param>
        /// <returns></returns>
        public static Task<TBase> ConvertResult<TBase, TDerived>(this Task<TDerived> task) where TDerived : TBase {
            return task.ContinueWith(
                                     t => (TBase)t.Result,
                                     TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion
                                    );
        }

        /// <summary>
        ///     Converts the result of a task using a selector method
        /// </summary>
        /// <typeparam name="TResult">The target type</typeparam>
        /// <typeparam name="TSource">The current type</typeparam>
        /// <param name="selectorFunc">The fucntion used to convert the result</param>
        /// <param name="task">The task</param>
        /// <returns></returns>
        public static Task<TResult> ConvertResult<TSource, TResult>(this Task<TSource> task, Func<TSource, TResult> selectorFunc) {
            return task.ContinueWith(
                                     (t) => selectorFunc.Invoke(t.Result), 
                                     TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion
                                     );
        }

        private static GetterDelegate CreateAccessor(Type taskType) {
            var propertyInfo = taskType.GetProperty("Result");
            return propertyInfo != null ? FluentIl.CreateGetter(propertyInfo) : null;
        }

        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> tasks)
        {
            var results = new List<T>();
            foreach (var valueTask in tasks) {
                if (valueTask.IsCompleted && valueTask.IsFaulted == false)
                    results.Add(valueTask.Result);
                else
                    results.Add(await valueTask);
            }

            return results;
        }
        public static async ValueTask<IEnumerable<T>> WhenAll<T>(this IEnumerable<ValueTask<T>> tasks)
        {
            var results = new List<T>();
            foreach (var valueTask in tasks) {
                if (valueTask.IsCompletedSuccessfully)
                    results.Add(valueTask.Result);
                else
                    results.Add(await valueTask);
            }

            return results;
        }
    }
}
