using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Rest.Services.Parameters;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.ResultWriters;
using DotLogix.Core.Rest.Services.Routing.Matching;

namespace DotLogix.Core.Rest.Services.Routing {
    public class WebServiceRouteBuilder {
        public string Pattern { get; set; }
        public bool IsRooted { get; set; }
        public int Priority { get; set; }
        public RouteMatchingStrategy MatchingStrategy { get; set; } = RouteMatchingStrategy.Equals;
        public HttpMethods AcceptedMethods { get; set; } = HttpMethods.Any;

        public List<IParameterProvider> ParameterProviders { get; } = new List<IParameterProvider>();
        public List<IRouteDescriptor> Descriptors { get; } = new List<IRouteDescriptor>();
        public List<IWebRequestProcessor> PostProcessors { get; } = new List<IWebRequestProcessor>();
        public List<IWebRequestProcessor> PreProcessors { get; } = new List<IWebRequestProcessor>();
        public IWebRequestProcessor RequestProcessor { get; set; }
        public IWebServiceResultWriter ResultWriter { get; set; } = PrimitiveResultWriter.Instance;

        #region Props
        public WebServiceRouteBuilder UseMethodFilter(HttpMethods acceptedMethods) {
            AcceptedMethods = acceptedMethods;
            return this;
        }
        public WebServiceRouteBuilder UsePriority(int priority) {
            Priority = priority;
            return this;
        }
        public WebServiceRouteBuilder UseRootScope(bool useRootScope) {
            IsRooted = useRootScope;
            return this;
        }

        public WebServiceRouteBuilder UsePattern(string pattern) {
            Pattern = pattern;
            return this;
        }
        public WebServiceRouteBuilder UsePattern(string pattern, RouteMatchingStrategy matchingStrategy) {
            Pattern = pattern;
            MatchingStrategy = matchingStrategy;
            return this;
        }
      
        public WebServiceRouteBuilder UseMatchingStrategy(RouteMatchingStrategy matchingStrategy) {
            MatchingStrategy = matchingStrategy;
            return this;
        }


        public WebServiceRouteBuilder UseProcessor(IWebRequestProcessor processor) {
            RequestProcessor = processor;
            return this;
        }

        public WebServiceRouteBuilder UseProcessor(Action<WebRequestProcessorBuilder> configureFunc) {
            var builder = new WebRequestProcessorBuilder();
            configureFunc.Invoke(builder);
            UseProcessor(builder.Build());
            return this;
        }

        public WebServiceRouteBuilder UseProcessor(Func<WebServiceContext, Task> handlerFunc, Func<WebServiceContext, bool> preConditionFunc = null, int priority = 0) {
            UseProcessor(new LambdaWebRequestProcessor(priority, handlerFunc, preConditionFunc));
            return this;
        }

        #endregion

        #region Decorators

        public WebServiceRouteBuilder UseDescriptor(IRouteDescriptor routeDescriptor) {
            Descriptors.Add(routeDescriptor);
            return this;
        }
        public WebServiceRouteBuilder UseDescriptor(Action<RouteDescriptorBuilder> configureFunc) {
            var builder = new RouteDescriptorBuilder();
            configureFunc.Invoke(builder);
            UseDescriptor(builder.Build());
            return this;
        }

        public WebServiceRouteBuilder UseDescriptors(params IRouteDescriptor[] routeDescriptors) {
            return UseDescriptors(routeDescriptors.AsEnumerable());
        }
        public WebServiceRouteBuilder UseDescriptors(IEnumerable<IRouteDescriptor> routeDescriptors) {
            Descriptors.AddRange(routeDescriptors);
            return this;
        }

        #endregion

        #region PreProcessor

        public WebServiceRouteBuilder UsePreProcessor(IWebRequestProcessor processor) {
            PreProcessors.Add(processor);
            return this;
        }

        public WebServiceRouteBuilder UsePreProcessor(Action<WebRequestProcessorBuilder> configureFunc) {
            var builder = new WebRequestProcessorBuilder();
            configureFunc.Invoke(builder);
            UsePreProcessor(builder.Build());
            return this;
        }

        public WebServiceRouteBuilder UsePreProcessors(IEnumerable<IWebRequestProcessor> processors) {
            PreProcessors.AddRange(processors);
            return this;
        }
        public WebServiceRouteBuilder UsePreProcessors(params IWebRequestProcessor[] processors) {
            return UsePreProcessors(processors.AsEnumerable());
        }

        #endregion


        #region PostProcessor

        public WebServiceRouteBuilder UsePostProcessor(IWebRequestProcessor processor) {
            PostProcessors.Add(processor);
            return this;
        }
        public WebServiceRouteBuilder UsePostProcessor(Action<WebRequestProcessorBuilder> configureFunc) {
            var builder = new WebRequestProcessorBuilder();
            configureFunc.Invoke(builder);
            UsePostProcessor(builder.Build());
            return this;
        }
        public WebServiceRouteBuilder UsePostProcessors(IEnumerable<IWebRequestProcessor> processors) {
            PostProcessors.AddRange(processors);
            return this;
        }
        public WebServiceRouteBuilder UsePostProcessors(params IWebRequestProcessor[] processors) {
            return UsePostProcessors(processors.AsEnumerable());
        }

        #endregion

        #region ParameterProviders

        public WebServiceRouteBuilder UseParameterProvider(IParameterProvider processor) {
            ParameterProviders.Add(processor);
            return this;
        }
        public WebServiceRouteBuilder UseParameterProviders(IEnumerable<IParameterProvider> providers) {
            ParameterProviders.AddRange(providers);
            return this;
        }
        public WebServiceRouteBuilder UseParameterProviders(params IParameterProvider[] providers) {
            return UseParameterProviders(providers.AsEnumerable());
        }

        #endregion

        public IWebServiceRoute Build(int routeIndex) {
            var matchingStrategy = SelectMatchingStrategy();
            var route = new WebServiceRoute(routeIndex, RequestProcessor, matchingStrategy, Priority) {
                ResultWriter = ResultWriter
            };
            route.Descriptors.AddRange(Descriptors);
            route.PreProcessors.AddRange(PreProcessors);
            route.PostProcessors.AddRange(PostProcessors);
            route.ParameterProviders.AddRange(ParameterProviders);
            return route;
        }
        
        private IRouteMatchingStrategy SelectMatchingStrategy() {
            return MatchingStrategy switch {
                                           RouteMatchingStrategy.Equals => new EqualsMatchingStrategy(Pattern, AcceptedMethods, IsRooted),
                                           RouteMatchingStrategy.StartsWith => new StartsWithMatchingStrategy(Pattern, AcceptedMethods, IsRooted),
                                           RouteMatchingStrategy.EndsWith => new EndsWithMatchingStrategy(Pattern, AcceptedMethods, IsRooted),
                                           RouteMatchingStrategy.Contains => new ContainsMatchingStrategy(Pattern, AcceptedMethods, IsRooted),
                                           RouteMatchingStrategy.Regex => new RegexMatchingStrategy(Pattern, AcceptedMethods, IsRooted),
                                           RouteMatchingStrategy.Pattern => new PatternMatchingStrategy(Pattern, AcceptedMethods, IsRooted),
                                           RouteMatchingStrategy.Fallback => FallbackMatchingStrategy.Instance,
                                           _ => throw new ArgumentOutOfRangeException(nameof(RouteMatchingStrategy), MatchingStrategy, null)
                                           };
        }
    }
}