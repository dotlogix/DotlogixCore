using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Reflection.Delegates;
using DotLogix.Core.Reflection.Fluent;

namespace DotLogix.Core.Extensions
{
    public static class TaskExtensions {
        private static readonly ConcurrentDictionary<Type, GetterDelegate> TypeResultAccessors = new ConcurrentDictionary<Type, GetterDelegate>();

        static TaskExtensions() {
            var voidType = Type.GetType("System.Threading.Tasks.VoidTaskResult");
            var taskOfVoid = typeof(Task<>).MakeGenericType(voidType);
            TypeResultAccessors.TryAdd(taskOfVoid, null);
            TypeResultAccessors.TryAdd(typeof(Task), null);
        }

        public static Task<object> UnpackResultAsync(this Task task)
        {
            if (task is Task<object> objectTask)
                return objectTask;

            var tcs = new TaskCompletionSource<object>();

            task.ContinueWith(t => tcs.SetResult(UnpackResult(t)), TaskContinuationOptions.OnlyOnRanToCompletion);
            // ReSharper disable once PossibleNullReferenceException
            task.ContinueWith(t => tcs.SetException(t.Exception.InnerExceptions), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => tcs.SetCanceled(), TaskContinuationOptions.OnlyOnCanceled);
            return tcs.Task;
        }

        public static object UnpackResult(this Task task) {
            if(task.IsCompleted == false)
                throw new InvalidOperationException("Task has to be completed to unpack the result");

            var taskType = task.GetType();
            var accessor = TypeResultAccessors.GetOrAdd(taskType, CreateAcessor);
            return accessor?.Invoke(task);
        }

        public static Task<TBase> ConvertResult<TBase, TDerived>(this Task<TDerived> task) where TDerived : TBase
        {
            var tcs = new TaskCompletionSource<TBase>();

            task.ContinueWith(t => tcs.SetResult(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            // ReSharper disable once PossibleNullReferenceException
            task.ContinueWith(t => tcs.SetException(t.Exception.InnerExceptions), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => tcs.SetCanceled(), TaskContinuationOptions.OnlyOnCanceled);
            return tcs.Task;
        }

        private static GetterDelegate CreateAcessor(Type taskType) { 
            var propertyInfo = taskType.GetProperty("Result");
            return propertyInfo != null ? FluentIl.CreateGetter(propertyInfo) : null;
        }
    }
}
