﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceHost.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  05.03.2018
// ==================================================

#region
using System;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Attributes.Descriptors;
using DotLogix.Core.Rest.Services.Attributes.Events;
using DotLogix.Core.Rest.Services.Attributes.ResultWriter;
using DotLogix.Core.Rest.Services.Attributes.Routes;
using DotLogix.Core.Rest.Services.Writer;
#endregion

namespace DotLogix.Core.Rest.Services {
    public class WebServiceHost {
        private readonly AsyncWebRequestRouter _router;
        private int _currentRouteIndex;
        public IAsyncHttpServer Server { get; }
        public WebRequestProcessorCollection GlobalPreProcessors => _router.GlobalPreProcessors;
        public WebRequestProcessorCollection GlobalPostProcessors => _router.GlobalPostProcessors;
        public WebServiceEventCollection ServerEvents => _router.ServerEvents;

        public WebServiceHost(Configuration configuration = null) {
            _router = new AsyncWebRequestRouter();
            Server = new AsyncHttpServer(_router, configuration);
        }

        public WebServiceHost(string urlPrefix, Configuration configuration = null) : this(configuration) {
            Server.AddServerPrefix(urlPrefix);
        }

        public void AddPrefix(string urlPrefix) {
            Server.AddServerPrefix(urlPrefix);
        }

        public void Start() {
            Log.Info($"HttpServer started with {_router.RegisteredRoutesCount} routes");
            Server.Start();
        }

        public void Stop() {
            Log.Info("HttpServer stopped");
            Server.Stop();
        }

        public void SetDefaultResultWriter(IAsyncWebRequestResultWriter writer) {
            _router.DefaultResultWriter = writer;
        }


        #region Events
        public Task TriggerEventAsync(string name, WebServiceEventArgs eventArgs) {
            if(_router.ServerEvents.TryGetValue(name, out var webServiceEvent) == false)
                throw new ArgumentException($"Event {name} is not defined", nameof(name));

            return webServiceEvent.TriggerAsync(this, eventArgs);
        }
        #endregion

        #region Services
        public void RegisterService(IWebService serviceInstance) {
            var serviceType = serviceInstance.GetType();
            var methods = serviceType.GetMethods();
            var count = 0;
            foreach(var methodInfo in methods) {
                var routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();
                if(routeAttribute == null)
                    continue;

                var dynamicInvoke = methodInfo.CreateDynamicInvoke();

                string route;
                if(string.IsNullOrEmpty(serviceInstance.RoutePrefix) == false)
                    route = serviceInstance.RoutePrefix + routeAttribute.Pattern;
                else
                    route = routeAttribute.Pattern;

                var serviceRoute = routeAttribute.BuildRoute(serviceInstance, _currentRouteIndex++, dynamicInvoke, route);
                if(serviceRoute == null)
                    continue;

                foreach(var preProcessorAttribute in methodInfo.GetCustomAttributes<PreProcessorAttribute>())
                    serviceRoute.PreProcessors.Add(preProcessorAttribute.CreateProcessor());

                foreach(var postProcessorAttribute in methodInfo.GetCustomAttributes<PostProcessorAttribute>())
                    serviceRoute.PostProcessors.Add(postProcessorAttribute.CreateProcessor());

                foreach (var descriptorAttribute in methodInfo.GetCustomAttributes<DescriptorAttribute>())
                    serviceRoute.RequestProcessor.Descriptors.Add(descriptorAttribute.CreateDescriptor());

                var resultWriterAttribute = methodInfo.GetCustomAttribute<RouteResultWriterAttribute>();
                if(resultWriterAttribute != null)
                    serviceRoute.WebRequestResultWriter = resultWriterAttribute.CreateResultWriter();

                _router.ServerRoutes.Add(serviceRoute);
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

                if(_router.ServerEvents.TryAdd(serviceEvent) == false)
                    throw new InvalidOperationException($"An event with name {serviceEvent.Name} is already defined");

                async void TriggerEvent(object sender, WebServiceEventArgs args) => await serviceEvent.TriggerAsync(sender, args);

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
