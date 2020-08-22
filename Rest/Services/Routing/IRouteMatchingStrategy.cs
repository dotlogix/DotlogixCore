using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Routing.Matching;

namespace DotLogix.Core.Rest.Services.Routing {
    public interface IRouteMatchingStrategy {
        bool IsRooted { get; }
        string Pattern { get; }
        HttpMethods AcceptedMethods { get; }
        RouteMatch Match(HttpMethods method, string path);
    }
}