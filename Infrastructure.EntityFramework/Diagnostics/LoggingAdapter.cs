using System;
using DotLogix.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Diagnostics {
    public class LoggingAdapter : ILogger {
        public LogLevels MinLogLevel { get; }

        public LoggingAdapter(LogLevels minLogLevel = LogLevels.Info) {
            MinLogLevel = minLogLevel;
        }

        /// <summary>Writes a log entry.</summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            if (eventId.Id == 10403 || this.IsEnabled(logLevel) == false)
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message) && exception == null)
                return;

            switch(logLevel) {
                case LogLevel.None:
                case LogLevel.Trace:
                    DotLogix.Core.Diagnostics.Log.Trace(message);
                    break;
                case LogLevel.Debug:
                case LogLevel.Information:
                    DotLogix.Core.Diagnostics.Log.Debug(message);
                    break;
                case LogLevel.Warning:
                    DotLogix.Core.Diagnostics.Log.Warn(message);
                    break;
                case LogLevel.Error:
                    if(exception != null)
                        DotLogix.Core.Diagnostics.Log.Error(exception);
                    else
                        DotLogix.Core.Diagnostics.Log.Error(message);
                    break;
                case LogLevel.Critical:
                    if (exception != null)
                        DotLogix.Core.Diagnostics.Log.Critical(exception);
                    else
                        DotLogix.Core.Diagnostics.Log.Critical(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel) {
            var logLevels = ConvertLogLevel(logLevel);
            return logLevels >= MinLogLevel && logLevels >= DotLogix.Core.Diagnostics.Log.LogLevel;
        }

        private LogLevels ConvertLogLevel(LogLevel logLevel) {
            switch(logLevel) {
                case LogLevel.None:
                case LogLevel.Trace:
                    return LogLevels.Trace;
                case LogLevel.Debug:
                    return LogLevels.Debug;
                case LogLevel.Information:
                    return LogLevels.Info;
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