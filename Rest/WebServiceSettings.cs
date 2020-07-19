using DotLogix.Core.Diagnostics;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Rest {
    public class WebServiceSettings : PrefixedSettings {
        public bool EnableMetrics {
            get => GetWithMemberName(true);
            set => SetWithMemberName(value);
        }

        public bool EnableLogging {
            get => GetWithMemberName(true);
            set => SetWithMemberName(value);
        }

        public ILogSource LogSource { get; }
        public WebServerSettings ServerSettings { get; }
        public WebServiceRouterSettings RouterSettings { get; }

        /// <inheritdoc />
        public WebServiceSettings() : base(new Settings()) { }

        public WebServiceSettings(ISettings settings, string prefix = null) : base(settings, prefix) {
            var hostLogSource = Log.CreateSource("WebService");
            var serverLogSource = hostLogSource.CreateSource("WebServer");
            var routerLogSource = hostLogSource.CreateSource("WebServiceRouter");

            LogSource = hostLogSource;
            ServerSettings = new WebServerSettings(Settings, string.Concat(Prefix, "webServer.")) { LogSource = serverLogSource };
            RouterSettings = new WebServiceRouterSettings(Settings, string.Concat(Prefix, "router.")) { LogSource = routerLogSource };
        }
    }
}