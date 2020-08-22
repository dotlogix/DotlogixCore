using DotLogix.Core.Rest.Http;

namespace DotLogix.Core.Rest.Services.Routing.Matching {
    public class EndsWithMatchingStrategy : IRouteMatchingStrategy {
        public RouteMatch Match(HttpMethods method, string path) {
            if(((method & AcceptedMethods) != 0) && path.EndsWith(Pattern))
                return new RouteMatch(true, path, Pattern.Length, null);
            return RouteMatch.Empty;
        }

        public EndsWithMatchingStrategy(string pattern, HttpMethods acceptedRequests, bool isRooted = false) {
            Pattern = pattern;
            AcceptedMethods = acceptedRequests;
            IsRooted = isRooted;
        }

        public bool IsRooted { get; }
        public string Pattern { get; }
        public HttpMethods AcceptedMethods { get; }
    }
}