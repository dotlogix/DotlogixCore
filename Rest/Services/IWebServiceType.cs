using System;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Services.Routing;

namespace DotLogix.Core.Rest.Services {
    public interface IWebServiceType {
        public string Name { get; }
        public string RoutePrefix { get; }
        public Type ServiceType { get; }
        public DynamicType DynamicServiceType { get; }
        public object Service { get; }
        public Func<object> ServiceFactory { get; }
        public WebServiceRouteCollection Routes { get; }
    }
}