using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Services.Processors {
    public class LambdaWebRequestProcessor : IWebRequestProcessor {
        public int Priority { get; }

        public LambdaWebRequestProcessor(int priority, Func<WebServiceContext, Task> handlerFunc, Func<WebServiceContext, bool> conditionFunc = null) {
            Priority = priority;
            _handlerFunc = handlerFunc ?? throw new ArgumentNullException(nameof(handlerFunc));
            _conditionFunc = conditionFunc;
        }

        /// <inheritdoc />
        public Task ProcessAsync(WebServiceContext context) => _handlerFunc.Invoke(context);

        /// <inheritdoc />
        public bool ShouldExecute(WebServiceContext webServiceContext) => _conditionFunc?.Invoke(webServiceContext) ?? true;

        private readonly Func<WebServiceContext, Task> _handlerFunc;
        private readonly Func<WebServiceContext, bool> _conditionFunc;
    }
}