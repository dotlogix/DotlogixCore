using DotLogix.Core.Diagnostics;
using DotLogix.Core.Rest.Events;
using DotLogix.Core.Rest.Services;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Rest.Services.Routing;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Rest {
    public class WebServiceRouterSettings : PrefixedSettings {
        /// <inheritdoc />
        public WebServiceRouterSettings() : this(null) { }

        public WebServiceRouterSettings(ISettings settings, string prefix = null) : base(settings, prefix) {
            LogSource ??= Log.CreateSource("WebServiceRouter");
            PreProcessors ??= new WebRequestProcessorCollection();
            PostProcessors ??= new WebRequestProcessorCollection();
            ParameterProviders ??= new ParameterProviderCollection(Services.Parameters.ParameterProviders.Context);
            DefaultResultWriter ??= PrimitiveResultWriter.Instance;
            Routes ??= new WebServiceRouteCollection();
            Events ??= new WebServiceEventCollection();
        }


        public ILogSource LogSource {
            get => GetWithMemberName<ILogSource>();
            set => SetWithMemberName(value);
        }

        public WebRequestProcessorCollection PreProcessors {
            get => GetWithMemberName(default(WebRequestProcessorCollection));
            set => SetWithMemberName(value);
        }
        public WebRequestProcessorCollection PostProcessors {
            get => GetWithMemberName<WebRequestProcessorCollection>();
            set => SetWithMemberName(value);
        }

        public ParameterProviderCollection ParameterProviders {
            get => GetWithMemberName<ParameterProviderCollection>();
            set => SetWithMemberName(value);
        }
        public IWebServiceResultWriter DefaultResultWriter {
            get => GetWithMemberName<IWebServiceResultWriter>();
            set => SetWithMemberName(value);
        }

        public WebServiceRouteCollection Routes {
            get => GetWithMemberName<WebServiceRouteCollection>();
            set => SetWithMemberName(value);
        }
        public WebServiceEventCollection Events {
            get => GetWithMemberName<WebServiceEventCollection>();
            set => SetWithMemberName(value);
        }
    }
}