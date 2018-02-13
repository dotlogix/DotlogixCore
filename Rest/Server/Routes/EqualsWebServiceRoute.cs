using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;

namespace DotLogix.Core.Rest.Server.Routes {
    public class EqualsWebServiceRoute : WebServiceRouteBase
    {
        public EqualsWebServiceRoute(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) : base(routeIndex, pattern, acceptedRequests, requestProcessor, priority) { }

        public override RouteMatch Match(HttpMethods method, string path)
        {
            if (((method & AcceptedRequests) != 0) && path.Equals(Pattern))
                return new RouteMatch(true, path, Pattern.Length, null);
            return RouteMatch.Empty;
        }
    }
}