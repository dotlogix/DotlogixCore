using System.Threading.Tasks;
using DotLogix.Core.Rest.Services.Context;
using DotLogix.Core.Rest.Services.Descriptors;

namespace DotLogix.Core.Rest.Services.Processors {
    public abstract class WebRequestProcessorBase : IWebRequestProcessor {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        protected WebRequestProcessorBase(int priority, bool ignoreHandled = true) {
            Priority = priority;
            IgnoreHandled = ignoreHandled;
            Descriptors = new WebRequestProcessorDescriptorCollection();
        }
        public WebRequestProcessorDescriptorCollection Descriptors { get; }
        public int Priority { get; }
        public bool IgnoreHandled { get; }

        public abstract Task ProcessAsync(WebServiceContext webServiceContext);

        public virtual bool ShouldExecute(WebServiceContext webServiceContext) {
            return webServiceContext.RequestResult.Handled == false || IgnoreHandled == false;
        }
    }
}