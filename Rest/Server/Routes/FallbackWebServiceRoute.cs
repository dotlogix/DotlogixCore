using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;

namespace DotLogix.Core.Rest.Server.Routes {
    public class FallbackWebServiceRoute : WebServiceRouteBase {
        public FallbackWebServiceRoute(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) : base(routeIndex, pattern, acceptedRequests, requestProcessor, priority) { }
        public override RouteMatch Match(HttpMethods method, string path) {
            return new RouteMatch(true, "", 0, null);
        }
    }
}