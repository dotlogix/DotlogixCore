using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Rest.Services.Processors {
    public class WebRequestProcessorBuilder {
        public int Priority { get; set; }
        public Func<WebServiceContext, Task> HandlerFunc { get; set; }
        public Func<WebServiceContext, bool> ConditionFunc { get; set; }
        
        public WebRequestProcessorBuilder UsePriority(int priority) {
            Priority = priority;
            return this;
        }

        public WebRequestProcessorBuilder UseHandler(Func<WebServiceContext, Task> handlerFunc) {
            HandlerFunc = handlerFunc;
            return this;
        }

        public WebRequestProcessorBuilder UseCondition(Func<WebServiceContext, bool> conditionFunc) {
            ConditionFunc = conditionFunc;
            return this;
        }

        public IWebRequestProcessor Build() {
            return new LambdaWebRequestProcessor(Priority, HandlerFunc, ConditionFunc);
        }
    }
}