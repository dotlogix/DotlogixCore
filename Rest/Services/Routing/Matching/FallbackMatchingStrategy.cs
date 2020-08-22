using DotLogix.Core.Rest.Http;

namespace DotLogix.Core.Rest.Services.Routing.Matching {
    public class FallbackMatchingStrategy : IRouteMatchingStrategy {
        public static IRouteMatchingStrategy Instance { get; } = new FallbackMatchingStrategy();

        public bool IsRooted => false;
        public string Pattern => "";
        public HttpMethods AcceptedMethods => HttpMethods.Any;

        public RouteMatch Match(HttpMethods method, string path) {
            return new RouteMatch(true, "", 0, null);
        }

        private FallbackMatchingStrategy() {
        }
    }
}