namespace DotLogix.Core.Rest.Services.Routing {
    public struct PrefixedRoute {
        public string Prefix { get; }
        public IWebServiceRoute Route { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public PrefixedRoute(IWebServiceRoute route, string prefix = null) {
            Prefix = route.IsRooted ? null : prefix;
            Route = route;
        }
    }
}