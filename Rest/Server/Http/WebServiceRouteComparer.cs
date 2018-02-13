using System.Collections.Generic;
using DotLogix.Core.Rest.Server.Routes;

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceRouteComparer : IComparer<IWebServiceRoute>
    {
        private WebServiceRouteComparer() { }

        public static IComparer<IWebServiceRoute> Instance { get; } = new WebServiceRouteComparer();
        public int Compare(IWebServiceRoute x, IWebServiceRoute y) {
            var priority = y.Priority.CompareTo(x.Priority);
            
            return priority == 0 ? y.RouteIndex.CompareTo(x.RouteIndex) : priority;
        }
    }
}