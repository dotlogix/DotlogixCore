using System;
using DotLogix.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Diagnostics {
    public class LoggingAdapter : ILogger {
        public LogLevel MinLogLevel { get; }

        public LoggingAdapter(LogLevel minLogLevel = LogLevel.Information) {
            MinLogLevel = minLogLevel;
        }

        /// <summary>Writes a log entry.</summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            var logLevels = ConvertLogLevel(logLevel);
            if (eventId.Id == 10403 || Core.Diagnostics.Log.LogLevel > logLevels)
                return;

            if(formatter == null)
                return;

            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message) && exception == null)
                return;

            if(exception != null)
                message += "\n" + exception.StackTrace;

            Core.Diagnostics.Log.Custom(logLevels, "EntityFramework Internal", nameof(LoggingAdapter), "EntityFramework", message);
        }

        bool ILogger.IsEnabled(LogLevel logLevel) {
            return logLevel >= MinLogLevel;
        }

        private LogLevels ConvertLogLevel(LogLevel logLevel) {
            switch(logLevel) {
                case LogLevel.None:
                case LogLevel.Trace:
                    return LogLevels.Trace;
                case LogLevel.Debug:
                case LogLevel.Information:
                    return LogLevels.Debug;
                case LogLevel.Warning:
                    return LogLevels.Warning;
                case LogLevel.Error:
                    return LogLevels.Error;
                case LogLevel.Critical:
                    return LogLevels.Critical;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        /// <summary>Begins a logical operation scope.</summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state) {
            return null;
        }
    }
}