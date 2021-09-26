using System;
using System.Reflection;
using System.Threading;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace DotLogix.WebServices.Core.Diagnostics {
    /// <summary>
    /// An implementation of the <see cref="DotLogix.Core.Diagnostics.ILogger"> interface for entity framework
    /// </summary>
    public class LoggerAdapter : Microsoft.Extensions.Logging.ILogger {
        /// <summary>
        /// The minimum entity framework log level
        /// </summary>
        public ILogSource LogSource { get; }

        public LoggerAdapter(ILogSource logSource) {
            LogSource = logSource;
        }


        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            var logLevels = ConvertLogLevel(logLevel);
            if(LogSource.LogLevel > logLevels)
                return;

            if(formatter == null)
                return;

            var message = formatter(state, exception);
            if(exception != null) {
                LogSource.Log(CreateLogMessage(logLevels, message, exception.TargetSite, exception));
            } else {
                LogSource.Custom(logLevels, "Undefined", LogSource.Name, "Undefined", message);
            }
        }

        bool Microsoft.Extensions.Logging.ILogger.IsEnabled(LogLevel logLevel) {
            return ConvertLogLevel(logLevel) >= LogSource.LogLevel;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) {
            return null;
        }

        private LogMessage CreateLogMessage(LogLevels logLevel, string message, MethodBase methodBase, Exception exception = null) {
            var type = methodBase.DeclaringType;
            var typeName = type?.GetFriendlyName() ?? "Undefined";
            var logMessage = new LogMessage {
                LogLevel = logLevel,
                UtcTimeStamp = DateTime.UtcNow,
                MethodName = methodBase.Name,
                ClassName = typeName,
                ThreadName = Thread.CurrentThread.Name ?? "Undefined",
                Message = message,
                Exception = exception,
                Source = LogSource
            };

            return logMessage;
        }
        private LogLevels ConvertLogLevel(LogLevel logLevel) {
            return logLevel switch {
                LogLevel.None => LogLevels.Trace,
                LogLevel.Trace => LogLevels.Trace,
                LogLevel.Debug => LogLevels.Debug,
                LogLevel.Information => LogLevels.Debug,
                LogLevel.Warning => LogLevels.Warning,
                LogLevel.Error => LogLevels.Error,
                LogLevel.Critical => LogLevels.Critical,
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
            };
        }
    }
}