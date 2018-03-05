using DotLogix.Core.Collections;
using DotLogix.Core.Rest.Services.Processors;

namespace DotLogix.Core.Rest.Server.Http {
    public class WebRequestProcessorCollection : SortedCollection<IWebRequestProcessor> {
        public WebRequestProcessorCollection() : base(WebRequestProcessorComparer.Instance) { }
    }
}