using System.Collections.Generic;
using DotLogix.Core.Rest.Services.Processors;

namespace DotLogix.Core.Rest.Server.Http {
    public class WebRequestProcessorComparer : IComparer<IWebRequestProcessor> {
        public static IComparer<IWebRequestProcessor> Instance { get; } = new WebRequestProcessorComparer();

        private WebRequestProcessorComparer() { }


        public int Compare(IWebRequestProcessor x, IWebRequestProcessor y) {
            return y.Priority.CompareTo(x.Priority);
        }
    }
}