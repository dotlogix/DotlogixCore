using System;
using DotLogix.Core.Collections;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Rest.Services.Routing;

namespace DotLogix.Core.Rest.Services {
    public class WebServiceSettings {
        private readonly KeyedCollection<Type, IWebServiceExtension> _extensions = new KeyedCollection<Type, IWebServiceExtension>(e => e.GetType());

        public bool EnableMetrics { get; set; } = true;
        public bool EnableLogging {
            get => LogSource.Enabled;
            set => LogSource.Enabled = value;
        }

        public ILogSource LogSource { get; }
        public WebServerSettings Server { get; }
        public WebServiceRouterSettings Router { get; }

        public WebServiceSettings() {
            var hostLogSource = Log.CreateSource("WebService");
            var serverLogSource = hostLogSource.CreateSource("WebServer");
            var routerLogSource = hostLogSource.CreateSource("WebServiceRouter");

            LogSource = hostLogSource;
            Server = new WebServerSettings {
                LogSource = serverLogSource
            };
            Router = new WebServiceRouterSettings {
                LogSource = routerLogSource
            };
        }

        public T UsePreProcessor<T>() where T : IWebServiceExtension, new() {
            return UseExtension(new T());
        }

        public T UseExtension<T>() where T : IWebServiceExtension, new() {
            return UseExtension(new T());
        }

        public T UseExtension<T>(T extension) where T : IWebServiceExtension {
            if(_extensions.TryGet(typeof(T), out var existing))
                return (T)existing;
            
            _extensions.Add(extension);
            extension.Configure(this);
            return extension;
        }

        public T GetExtension<T>() where T : IWebServiceExtension {
            if (_extensions.TryGet(typeof(T), out var existing))
                return (T)existing;
            return default;
        }
    }
}
