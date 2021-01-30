// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceHost.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Events;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Routing;
#endregion

namespace DotLogix.Core.Rest.Services {
    public class WebServiceHost : IWebServiceHost {
        private int _currentRouteIndex;
        public WebServiceSettings Settings { get; }
        public ILogSource LogSource => Settings.LogSource;
        public IAsyncWebServer Server { get; }
        public WebServiceRouter Router { get; }
        public WebServiceCollection Services { get; }

        public WebServiceHost(WebServiceSettings settings = null) {
            Settings = settings ?? new WebServiceSettings();
            Router = new WebServiceRouter(Settings);
            Server = new AsyncWebServer(Router, Settings.Server);
            Services = new WebServiceCollection();
        }

        public WebServiceHost(string urlPrefix, WebServiceSettings settings = null) : this(settings) {
            Settings.Server.UrlPrefixes.Add(urlPrefix);
        }

        public void Start() {
            LogSource.Info($"HttpServer started with {Router.Routes.Count} routes listening on {string.Join(", ", Server.Settings.UrlPrefixes)}");
            Server.Start();
        }

        public void Stop() {
            LogSource.Info("HttpServer stopped");
            Server.Stop();
        }


        #region Events
        public Task TriggerEventAsync(string name, WebServiceEventArgs eventArgs) {
            if(Router.Events.TryGet(name, out var webServiceEvent) == false)
                throw new ArgumentException($"Event {name} is not defined", nameof(name));

            return webServiceEvent.DispatchAsync(this, eventArgs);
        }
        #endregion

        #region Services
        public void RegisterService(IWebService serviceInstance, Action<WebServiceBuilder> configureFunc = null) {
            if(Services.TryAdd(serviceInstance) == false)
                throw new ArgumentException($"A service with name {serviceInstance.Name} is already registered. Choose another name");

            var serviceType = serviceInstance.GetType();

            LogSource.Trace($"Register webservice {serviceInstance.Name}");

            void ConfigureService(WebServiceBuilder b) {
                b.UseService(serviceInstance);
                configureFunc?.Invoke(b);
            }

            RegisterService(serviceType, ConfigureService);
            RegisterEvents(serviceInstance, serviceType);
        }
        public void RegisterService(Type serviceType, Func<IWebService> serviceFactory, Action<WebServiceBuilder> configureFunc = null) {
            void ConfigureService(WebServiceBuilder b) {
                b.UseService(serviceFactory);
                configureFunc?.Invoke(b);
            }

            RegisterService(serviceType, ConfigureService);
        }
        public void RegisterService(Type serviceType, Action<WebServiceBuilder> configureFunc) {
            var builder = new WebServiceBuilder(serviceType);
            configureFunc.Invoke(builder);
            
            var methods = serviceType.GetMethods();
            foreach (var methodInfo in methods) {
                var routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();
                if (routeAttribute == null)
                    continue;
                builder.UseRoute(methodInfo);
            }
            
            RegisterService(builder);
        }
        public void RegisterService(WebServiceBuilder builder) {
            var webService = builder.Build(_currentRouteIndex);
            foreach (var prefixedRoute in webService.Routes) {
                var route = prefixedRoute.Route;
                var prefix = prefixedRoute.Prefix;

                Router.Routes.Add(route, prefix);
                var descriptor = route.Descriptors.GetCustomDescriptor<MethodDescriptor>();
                LogSource.Trace($"Registered webservice route {webService.Name}.{descriptor?.DynamicInvoke.Name ?? route.Pattern} as {prefix ?? ""}{route.Pattern}");
            }

            var count = builder.Routes.Count();
            if(count > 0)
                LogSource.Debug($"Registered webservice {webService.Name} with {count} routes");
            
            _currentRouteIndex += count;
        }

        public void RegisterService<TService>(Func<TService> serviceFactory, Action<WebServiceBuilder> configureFunc = null) where TService : class, IWebService {
            RegisterService(typeof(TService), serviceFactory, configureFunc);
        }
        public void RegisterService<TService>(Action<WebServiceBuilder> configureFunc = null) where TService : class, IWebService, new() {
            RegisterService(new TService(), configureFunc);
        }

        public void RegisterRoute(Action<WebServiceRouteBuilder> configureFunc) {
            var routeBuilder = new WebServiceRouteBuilder();
            routeBuilder.UseRootScope(true);
            configureFunc.Invoke(routeBuilder);

            var route = routeBuilder.Build(_currentRouteIndex++);
            Router.Routes.Add(route);
        }

        private void RegisterEvents(IWebService serviceInstance, Type serviceType) {
            var count = 0;
            var events = serviceType.GetEvents();
            foreach(var eventInfo in events) {
                var eventAttribute = eventInfo.GetCustomAttribute<WebServiceEventAttributeBase>();
                if(eventAttribute == null)
                    continue;

                var serviceEvent = eventAttribute.CreateEvent();

                if(Router.Events.TryAdd(serviceEvent) == false)
                    throw new InvalidOperationException($"An event with name {serviceEvent.Name} is already defined");

                async void TriggerEvent(object sender, WebServiceEventArgs args) => await serviceEvent.DispatchAsync(sender, args);

                eventInfo.GetAddMethod().CreateDynamicInvoke().Invoke(serviceInstance, (EventHandler<WebServiceEventArgs>)TriggerEvent);
                count++;
            }

            if(count > 0)
                LogSource.Debug($"Bound {count} events to webservice {serviceInstance.Name}");
        }
        #endregion
    }
}
