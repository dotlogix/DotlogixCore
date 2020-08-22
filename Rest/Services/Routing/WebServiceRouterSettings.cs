using DotLogix.Core.Diagnostics;
using DotLogix.Core.Rest.Events;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.ResultWriters;

namespace DotLogix.Core.Rest.Services.Routing {
    public class WebServiceRouterSettings {
        public bool EnableLogging {
            get => LogSource.Enabled;
            set => LogSource.Enabled = value;
        }
        public ILogSource LogSource { get; set; } = Log.CreateSource("WebServiceRouter");

        public WebRequestProcessorCollection PreProcessors { get; } = new WebRequestProcessorCollection();
        public WebRequestProcessorCollection PostProcessors { get; } = new WebRequestProcessorCollection();
        public ParameterProviderCollection ParameterProviders { get; } = new ParameterProviderCollection(Services.Parameters.ParameterProviders.Context);
        public IWebServiceResultWriter DefaultResultWriter { get; set; } = PrimitiveResultWriter.Instance;

        public WebServiceRouteCollection Routes { get; } = new WebServiceRouteCollection();
        public WebServiceEventCollection Events { get; } = new WebServiceEventCollection();
    }
}