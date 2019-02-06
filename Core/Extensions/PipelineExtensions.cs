using System;
using System.Threading.Tasks;
using DotLogix.Core.Patterns;

namespace DotLogix.Core.Extensions {
    public static class PipelineExtensions {
        public static void Add<TIn, TOut>(this IPipeline<TIn, TOut> pipeline, Func<TIn, Func<TIn, Task<Optional<TOut>>>, Task<Optional<TOut>>> callbackFunc) {
            pipeline.Add(new CallbackPipelineStep<TIn, TOut>(callbackFunc));
        }

        public static void Add<TIn, TNext, TOut>(this IPipeline<TIn, TOut> pipeline, Func<TNext, Func<TIn, Task<Optional<TOut>>>, Task<Optional<TOut>>> callbackFunc, Func<TIn, TNext> transformFunc) {
            pipeline.Add(new CallbackPipelineStep<TIn, TOut>((value, next) => callbackFunc.Invoke(transformFunc.Invoke(value), next)));
        }
    }
}