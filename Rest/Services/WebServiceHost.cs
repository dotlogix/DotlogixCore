// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceHost.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Attributes.Events;
using DotLogix.Core.Rest.Services.Attributes.Routes;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Services {
    public class WebServiceHost {
        private readonly AsyncWebRequestRouter _router;
        public IAsyncHttpServer Server { get; }

        public WebServiceHost(ConcurrencyOptions concurrencyOptions = null) {
            _router = new AsyncWebRequestRouter();
            Server = new AsyncHttpServer(_router, concurrencyOptions);
        }

        public WebServiceHost(string urlPrefix, ConcurrencyOptions concurrencyOptions = null) : this(concurrencyOptions) {
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

        public void SetDefaultResultWriter(IWebRequestResultWriter writer) {
            _router.DefaultResultWriter = writer;
        }

        #region Processors
        public void AddGlobalPreProcessor(IWebRequestProcessor preProcessor) {
            _router.AddGlobalPreProcessor(preProcessor);
        }

        public void RemoveGlobalPreProcessor(IWebRequestProcessor preProcessor) {
            _router.RemoveGlobalPreProcessor(preProcessor);
        }

        public void AddGlobalPostProcessor(IWebRequestProcessor postProcessor) {
            _router.AddGlobalPostProcessor(postProcessor);
        }

        public void RemoveGlobalPostProcessor(IWebRequestProcessor postProcessor) {
            _router.RemoveGlobalPostProcessor(postProcessor);
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

                var serviceRoute = routeAttribute.BuildRoute(serviceInstance, dynamicInvoke, route);
                if(serviceRoute == null)
                    continue;

                foreach(var preProcessorAttribute in methodInfo.GetCustomAttributes<PreProcessorAttribute>())
                    serviceRoute.AddPreProcessor(preProcessorAttribute.CreateProcessor());

                foreach(var postProcessorAttribute in methodInfo.GetCustomAttributes<PostProcessorAttribute>())
                    serviceRoute.AddPostProcessor(postProcessorAttribute.CreateProcessor());

                _router.AddServerRoute(serviceRoute);
                count++;
            }
            if(count > 0)
                Log.Debug($"Registered webservice {serviceInstance.Name} with {count} methods");


            count = 0;
            var events = serviceType.GetEvents();
            foreach(var eventInfo in events) {
                var eventAttribute = eventInfo.GetCustomAttribute<WebServiceEventAttribute>();
                if (eventAttribute == null)
                    continue;

                var serviceEvent = _router.GetOrAddServerEvent(eventAttribute.Name);

                async void TriggerEvent(object sender, EventArgs args) => await serviceEvent.TriggerEventAsync();

                eventInfo.GetAddMethod().CreateDynamicInvoke().Invoke(serviceInstance, (EventHandler)TriggerEvent);
                count++;
            }

            if (count > 0)
                Log.Debug($"Bound {count} events to webservice {serviceInstance.Name}");
        }

        public void RegisterService<TService>() where TService : class, IWebService, new() {
            RegisterService(new TService());
        }
        #endregion


        #region Events
        public WebServiceEvent AddServerEvent(string name) {
            return _router.AddServerEvent(name);
        }

        public WebServiceEvent GetServerEvent(string name)
        {
            return _router.GetServerEvent(name);
        }

        public WebServiceEvent GetOrAddServerEvent(string name)
        {
            return _router.GetOrAddServerEvent(name);
        }

        public Task TriggerServerEventAsync(string name)
        {
            return _router.TriggerServerEventAsync(name);
        }
        #endregion
    }
}
