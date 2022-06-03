using System;
using System.Collections.Generic;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Options;
using Microsoft.Extensions.Options;

namespace DotLogix.WebServices.Core.Diagnostics {
    public class ConfigLogSourceProvider : LogSourceProviderBase, IDisposable {
        private readonly IDisposable _optionChangeSubscription;
        private readonly IOptionsMonitor<LoggerOptions> _options;

        private readonly object _cacheLock = new();
        private readonly Dictionary<string, LogLevels> _cache = new();
        private LogLevels _defaultLogLevel;
        public LoggerOptions Options => _options.CurrentValue;

        public ConfigLogSourceProvider(ILogger logger, IOptionsMonitor<LoggerOptions> options) : base(logger) {
            _options = options;
            _optionChangeSubscription = options.OnChange(LoggerOptions_OnChange);
            LoggerOptions_OnChange(options.CurrentValue);
        }

        protected override LogLevels GetLogLevel(ILogSource logSource) {
            lock(_cacheLock) {
                return GetLogLevelRecursive(logSource.Name, _cache);
            }
        }

        private LogLevels GetLogLevelRecursive(string name, IDictionary<string, LogLevels> cache) {
            if(cache.TryGetValue(name, out var logLevel)) {
                return logLevel;
            }

            var index = name.LastIndexOf('.');
            if(index > 0) {
                var parentName = name.Substring(0, index);
                logLevel = GetLogLevelRecursive(parentName, cache);
            } else {
                logLevel = _defaultLogLevel;
            }

            cache[name] = logLevel;
            return logLevel;
        }

        private void LoggerOptions_OnChange(LoggerOptions newOptions) {
            lock(_cacheLock) {
                if(newOptions is null) {
                    _defaultLogLevel = LogLevels.Info;
                    _cache.Clear();
                } else {
                    _defaultLogLevel = newOptions.LogLevel;
                    _cache.Clear();
                    _cache.AddRange(newOptions.Sources);
                }
            }
        }
        
        public void Dispose() {
            _optionChangeSubscription?.Dispose();
        }
    }
}
