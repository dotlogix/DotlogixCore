using DotLogix.Core.Collections;
using DotLogix.Core.Rest.Server.Routes;

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceRouteCollection : SortedCollection<IWebServiceRoute> {
        public WebServiceRouteCollection() : base(WebServiceRouteComparer.Instance) { }
    }
}