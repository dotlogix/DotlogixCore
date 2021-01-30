using System;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Rest.Services.Routing;

namespace DotLogix.Core.Rest.Services {
    public interface IWebServiceHost {
        WebServiceSettings Settings { get; }
        ILogSource LogSource { get; }
        IAsyncWebServer Server { get; }
        WebServiceRouter Router { get; }
        WebServiceCollection Services { get; }
        void Start();
        void Stop();
        void RegisterService(IWebService serviceInstance, Action<WebServiceBuilder> configureFunc = null);
        void RegisterService(Type serviceType, Func<IWebService> serviceFactory, Action<WebServiceBuilder> configureFunc = null);
        void RegisterService(Type serviceType, Action<WebServiceBuilder> configureFunc);
        void RegisterService(WebServiceBuilder builder);
        void RegisterService<TService>(Func<TService> serviceFactory, Action<WebServiceBuilder> configureFunc = null) where TService : class, IWebService;
        void RegisterService<TService>(Action<WebServiceBuilder> configureFunc = null) where TService : class, IWebService, new();
        void RegisterRoute(Action<WebServiceRouteBuilder> configureFunc);
    }
}