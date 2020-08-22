using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.ResultWriters;
using DotLogix.Core.Rest.Services.Routing;

namespace DotLogix.Core.Rest.Services {
    public class WebServiceTypeBuilder {
        private readonly Dictionary<MethodInfo, WebServiceRouteBuilder> _methodMapping = new Dictionary<MethodInfo, WebServiceRouteBuilder>();
        private readonly List<WebServiceRouteBuilder> _routeBuilders = new List<WebServiceRouteBuilder>();
        public string Name { get; set; }
        public string RoutePrefix { get; set; }
        public Type ServiceType { get; }
        public DynamicType DynamicServiceType { get; }
        public object Service { get; set; }
        public Func<object> ServiceFactory { get; set; }
        public IEnumerable<WebServiceRouteBuilder> Routes => _routeBuilders;

        public WebServiceTypeBuilder(DynamicType serviceType) {
            ServiceType = serviceType.Type;
            DynamicServiceType = serviceType;

            var attribute = ServiceType.GetCustomAttribute<WebServiceAttribute>();
            if(attribute != null) {
                Name = attribute.Name;
                RoutePrefix = attribute.Route;
            }
        }
        public WebServiceTypeBuilder(Type serviceType) : this(serviceType.CreateDynamicType()) {
        }

        public WebServiceTypeBuilder UseName(string name) {
            Name = name;
            return this;
        }

        public WebServiceTypeBuilder UseRoutePrefix(string routePrefix) {
            RoutePrefix = routePrefix;
            return this;
        }


        public WebServiceTypeBuilder UseService(object service) {
            if(_methodMapping.Count > 0)
                throw new InvalidOperationException("Can not change service instance after configuring routes");

            Service = service;
            ServiceFactory = null;
            return this;
        } 

        public WebServiceTypeBuilder UseServiceFactory(Func<object> serviceFactory) {
            if (_methodMapping.Count > 0)
                throw new InvalidOperationException("Can not change service factory after configuring routes");

            Service = null;
            ServiceFactory = serviceFactory;
            return this;
        }

        public WebServiceRouteBuilder UseRoute(string methodName) {
            if (Service == null && ServiceFactory == null)
                throw new InvalidOperationException("Can not configure routes without a service instance or factory");

            var methodInfo = ServiceType.GetMethod(methodName);
            if (methodInfo == null)
                throw new ArgumentException($"Method {methodName} can not be found in service type {ServiceType.Name}");

            return UseRoute(methodInfo);
        }

        public WebServiceTypeBuilder UseRoute(string methodName, Action<WebServiceRouteBuilder> configureFunc) {
            var routeBuilder = UseRoute(methodName);
            configureFunc?.Invoke(routeBuilder);
            return this;
        }
        
        public WebServiceRouteBuilder UseRoute(MethodInfo methodInfo) {
            if (Service == null && ServiceFactory == null)
                throw new InvalidOperationException("Can not configure routes without a service instance or factory");

            if (ServiceType.IsAssignableTo(methodInfo.DeclaringType)== false)
                throw new ArgumentException($"Method {methodInfo.Name} can not be found in service type {ServiceType.Name}");

            if (_methodMapping.TryGetValue(methodInfo, out var routeBuilder) == false) {
                routeBuilder = CreateRouteBuilder(methodInfo);
                _methodMapping.Add(methodInfo, routeBuilder);
                _routeBuilders.Add(routeBuilder);
            }

            return routeBuilder;
        }

        public WebServiceTypeBuilder UseRoute(MethodInfo methodInfo, Action<WebServiceRouteBuilder> configureFunc) {
            var routeBuilder = UseRoute(methodInfo);
            configureFunc?.Invoke(routeBuilder);
            return this;
        }
        
        public void UseRoute(Action<WebServiceRouteBuilder> configureFunc) {
            var routeBuilder = new WebServiceRouteBuilder();
            configureFunc.Invoke(routeBuilder);
            _routeBuilders.Add(routeBuilder);
        }

        public IWebServiceType Build(int serviceRouteOffset) {
            var webServiceType = Service != null
                                 ? new WebServiceType(DynamicServiceType, Service) 
                                 : new WebServiceType(DynamicServiceType, ServiceFactory);

            for(var i = 0; i < _routeBuilders.Count; i++) {
                var route = _routeBuilders[i];
                webServiceType.Routes.Add(route.Build(serviceRouteOffset + i), RoutePrefix);
            }
            return webServiceType;
        }

        private WebServiceRouteBuilder CreateRouteBuilder(MethodInfo methodInfo) {
            var dynamicInvoke = DynamicServiceType.Resolve(methodInfo);
            if(dynamicInvoke == null)
                throw new ArgumentException($"Method {methodInfo.Name} can not be found in service type {ServiceType.Name}");

            var routeAttribute = GetRouteAttribute(dynamicInvoke);
            var acceptedMethods = GetHttpMethods(dynamicInvoke);
            var requestProcessor = GetRequestProcessor(dynamicInvoke);
            var resultWriter = GetResultWriter(dynamicInvoke);
            var preProcessors = GetPreProcessors(dynamicInvoke);
            var postProcessors = GetPostProcessors(dynamicInvoke);
            var descriptors = GetDescriptors(dynamicInvoke);

            var routeBuilder = new WebServiceRouteBuilder {
                Priority = routeAttribute.Priority,
                AcceptedMethods = acceptedMethods,
                Pattern = routeAttribute.Pattern,
                MatchingStrategy = routeAttribute.MatchingStrategy,
                IsRooted = routeAttribute.IsRooted,

                RequestProcessor = requestProcessor,
                ResultWriter = resultWriter
            };
            routeBuilder.UsePreProcessors(preProcessors);
            routeBuilder.UsePostProcessors(postProcessors);
            routeBuilder.UseDescriptor(new MethodDescriptor(dynamicInvoke));
            routeBuilder.UseDescriptors(descriptors);
            return routeBuilder;
        }

        protected virtual RouteAttribute GetRouteAttribute(DynamicInvoke dynamicInvoke) {
            var routeAttribute = dynamicInvoke.MethodInfo.GetCustomAttribute<RouteAttribute>();
            if(routeAttribute == null)
                throw new ArgumentException($"Method {dynamicInvoke.MethodInfo.Name} in type {ServiceType.Name} does not have a route attribute");
            return routeAttribute;
        }


        protected virtual HttpMethods GetHttpMethods(DynamicInvoke dynamicInvoke) {
            var httpMethods = dynamicInvoke
                             .MethodInfo
                             .GetCustomAttributes<HttpMethodAttribute>()
                             .Aggregate(HttpMethods.None,
                                        (m, a) => m | a.Methods
                                       );

            if(httpMethods == HttpMethods.None)
                throw new ArgumentException($"Method {dynamicInvoke.MethodInfo.Name} in type {ServiceType.Name} has to define at least one http method");
            return httpMethods;
        }

        protected virtual IEnumerable<IRouteDescriptor> GetDescriptors(DynamicInvoke dynamicInvoke) {
            return dynamicInvoke
                  .MethodInfo
                  .GetCustomAttributes<DescriptorAttribute>()
                  .Select(d => d.CreateDescriptor());
        }

        protected virtual IWebServiceResultWriter GetResultWriter(DynamicInvoke dynamicInvoke) {
            var resultWriterAttribute = dynamicInvoke
                                       .MethodInfo
                                       .GetCustomAttribute<RouteResultWriterAttribute>();
            return resultWriterAttribute.CreateResultWriter();
        }

        protected virtual IWebRequestProcessor GetRequestProcessor(DynamicInvoke dynamicInvoke) {
            return Service != null
                       ? new WebRequestProcessor(Service, dynamicInvoke, 0, false)
                       : new WebRequestProcessor(ServiceFactory, dynamicInvoke, 0, false);
        }

        protected virtual IEnumerable<IWebRequestProcessor> GetPreProcessors(DynamicInvoke dynamicInvoke) {
            return dynamicInvoke
                  .MethodInfo
                  .GetCustomAttributes<PreProcessorAttribute>()
                  .Select(d => d.CreateProcessor());
        }

        protected virtual IEnumerable<IWebRequestProcessor> GetPostProcessors(DynamicInvoke dynamicInvoke) {
            return dynamicInvoke
                  .MethodInfo
                  .GetCustomAttributes<PostProcessorAttribute>()
                  .Select(d => d.CreateProcessor());
        }
    }
}