using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Patterns {
    public class CallbackPipelineStep<TIn, TOut> : PipelineStep<TIn, TOut> {
        private readonly Func<TIn, Func<TIn, Task<Optional<TOut>>>, Task<Optional<TOut>>> _callbackFunc;

        public CallbackPipelineStep(Func<TIn, Func<TIn, Task<Optional<TOut>>>, Task<Optional<TOut>>> callbackFunc) {
            _callbackFunc = callbackFunc ?? throw new ArgumentNullException(nameof(callbackFunc));
        }
        public override Task<Optional<TOut>> Execute(TIn value) {
            return _callbackFunc.Invoke(value, Next);
        }
    }
}