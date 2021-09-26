using System;
using System.Collections.Concurrent;
using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Core.Options;
using Microsoft.Extensions.Options;

namespace DotLogix.WebServices.Core.Diagnostics {
    public class ConfigLogSourceProvider : LogSourceProviderBase, IDisposable{
        private readonly IDisposable _handler;
        private readonly IOptionsMonitor<LoggerOptions> _options;
        private ConcurrentDictionary<string, LogLevels> _cache;

        public LoggerOptions Options => _options.CurrentValue;

        public ConfigLogSourceProvider(ILogger logger, IOptionsMonitor<LoggerOptions> options) : base(logger) {
            _options = options;
            _cache = new ConcurrentDictionary<string, LogLevels>(options.CurrentValue.Sources);
            _handler = options.OnChange((o) => {
                                 _cache = new ConcurrentDictionary<string, LogLevels>(o.Sources);
                             });
        }

        protected override LogLevels GetLogLevel(string name) {
            return GetLogLevelRecursive(name, _cache);
        }

        private LogLevels GetLogLevelRecursive(string name, ConcurrentDictionary<string, LogLevels> cache) {
            if(cache.TryGetValue(name, out var logLevel)) {
                return logLevel;
            }

            var index = name.LastIndexOf('.');
            if(index > 0) {
                var parentName = name.Substring(0, index);
                logLevel = GetLogLevelRecursive(parentName, cache);
            } else {
                logLevel = Options.LogLevel;
            }

            return cache.GetOrAdd(name, logLevel);
        }

        public void Dispose() {
            _handler?.Dispose();
        }
    }
}
