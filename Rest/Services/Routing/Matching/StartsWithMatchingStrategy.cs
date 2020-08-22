using DotLogix.Core.Rest.Http;

namespace DotLogix.Core.Rest.Services.Routing.Matching {
    public class StartsWithMatchingStrategy : IRouteMatchingStrategy {
        public RouteMatch Match(HttpMethods method, string path) {
            if(((method & AcceptedMethods) != 0) && path.StartsWith(Pattern))
                return new RouteMatch(true, path, Pattern.Length, null);
            return RouteMatch.Empty;
        }

        public StartsWithMatchingStrategy(string pattern, HttpMethods acceptedRequests, bool isRooted = false) {
            Pattern = pattern;
            AcceptedMethods = acceptedRequests;
            IsRooted = isRooted;
        }

        public bool IsRooted { get; }
        public string Pattern { get; }
        public HttpMethods AcceptedMethods { get; }
    }
}