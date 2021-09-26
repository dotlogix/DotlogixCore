using System;

namespace DotLogix.Core.Diagnostics {
    public class LogSourceProvider : LogSourceProviderBase {
        private readonly Func<string, LogLevels> _getLogLevel;

        public LogSourceProvider(ILogger logger, Func<string, LogLevels> getLogLevel = null)
            : base(logger) {
            _getLogLevel = getLogLevel;
        }

        protected override LogLevels GetLogLevel(string name) {
            return _getLogLevel?.Invoke(name) ?? Log.LogLevel;
        }
    }
}