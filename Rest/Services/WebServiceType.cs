using System;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Services.Routing;

namespace DotLogix.Core.Rest.Services {
    public class WebServiceType : IWebServiceType {
        public string Name { get; }
        public string Route { get; }
        public Type ServiceType { get; }
        public DynamicType DynamicServiceType { get; }
        public object Service { get; }
        public Func<object> ServiceFactory { get; }
        public WebServiceRouteCollection Routes { get; } = new WebServiceRouteCollection();
        
        public WebServiceType(DynamicType dynamicServiceType, object service) {
            ServiceType = dynamicServiceType.Type;
            DynamicServiceType = dynamicServiceType;
            Service = service;
        }

        public WebServiceType(DynamicType dynamicServiceType, Func<object> serviceFactory = null) {
            ServiceType = dynamicServiceType.Type;
            DynamicServiceType = dynamicServiceType;
            ServiceFactory = serviceFactory;
        }
    }
}