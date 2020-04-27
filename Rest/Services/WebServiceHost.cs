// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceHost.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Events;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Routing;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Rest.Services {
    public class WebServiceHost {
        public ISettings Settings { get; } = new Settings();
        public WebServiceRouter Router { get; }
        private int _currentRouteIndex;
        public IAsyncWebServer Server { get; }
        public WebRequestProcessorCollection GlobalPreProcessors => Router.PreProcessors;
        public WebRequestProcessorCollection GlobalPostProcessors => Router.PostProcessors;
        public WebServiceEventCollection ServerEvents => Router.Events;
        public WebServiceCollection Services { get; }

        public WebServiceHost(WebServerConfiguration configuration = null) {
            Router = new WebServiceRouter();
            Server = new AsyncWebServer(Router, configuration);
            Services = new WebServiceCollection();
        }

        public WebServiceHost(string urlPrefix, WebServerConfiguration configuration = null) : this(configuration) {
            Server.AddServerPrefix(urlPrefix);
        }

        public void AddPrefix(string urlPrefix) {
            Server.AddServerPrefix(urlPrefix);
        }

        public void Start() {
            Log.Info($"HttpServer started with {Router.Routes.Count} routes");
            Server.Start();
        }

        public void Stop() {
            Log.Info("HttpServer stopped");
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
        public void RegisterService(IWebService serviceInstance) {
            if(Services.TryAdd(serviceInstance) == false)
                throw new ArgumentException($"A service with name {serviceInstance.Name} is already registered. Choose another name");

            var serviceType = serviceInstance.GetType();
            var methods = serviceType.GetMethods();
            var count = 0;
            foreach(var methodInfo in methods) {
                var routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();
                if(routeAttribute == null)
                    continue;

                var dynamicInvoke = methodInfo.CreateDynamicInvoke();

                var serviceRoute = routeAttribute.BuildRoute(serviceInstance, _currentRouteIndex++, dynamicInvoke, routeAttribute.Pattern);
                if(serviceRoute == null)
                    continue;

                foreach (var preProcessorAttribute in methodInfo.GetCustomAttributes<PreProcessorAttribute>())
                    serviceRoute.PreProcessors.Add(preProcessorAttribute.CreateProcessor());

                foreach(var postProcessorAttribute in methodInfo.GetCustomAttributes<PostProcessorAttribute>())
                    serviceRoute.PostProcessors.Add(postProcessorAttribute.CreateProcessor());

                foreach(var descriptorAttribute in methodInfo.GetCustomAttributes<DescriptorAttribute>())
                    serviceRoute.Descriptors.Add(descriptorAttribute.CreateDescriptor());
                
                serviceRoute.Descriptors.Add(new SettingsDescriptor(Settings));

                var resultWriterAttribute = methodInfo.GetCustomAttribute<RouteResultWriterAttribute>();
                if(resultWriterAttribute != null)
                    serviceRoute.WebServiceResultWriter = resultWriterAttribute.CreateResultWriter();


                Router.Routes.Add(serviceRoute, serviceInstance.RoutePrefix ?? "");
                count++;
            }
            if(count > 0)
                Log.Debug($"Registered webservice {serviceInstance.Name} with {count} methods");


            count = 0;
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
                Log.Debug($"Bound {count} events to webservice {serviceInstance.Name}");
        }

        public void RegisterService<TService>() where TService : class, IWebService, new() {
            RegisterService(new TService());
        }
        #endregion
    }
}
