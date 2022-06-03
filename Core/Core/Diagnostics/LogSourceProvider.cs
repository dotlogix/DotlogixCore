using System;

namespace DotLogix.Core.Diagnostics {
    public class LogSourceProvider : LogSourceProviderBase {
        private readonly Func<ILogSource, LogLevels> _getLogLevel;

        public LogSourceProvider(ILogger logger, Func<ILogSource, LogLevels> getLogLevel) : base(logger) {
            _getLogLevel = getLogLevel;
        }

        protected override LogLevels GetLogLevel(ILogSource logSource) {
            return _getLogLevel.Invoke(logSource);
        }
    }
}