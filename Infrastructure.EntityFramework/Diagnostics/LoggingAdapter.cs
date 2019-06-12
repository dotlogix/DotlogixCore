// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LoggingAdapter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Diagnostics {
    /// <summary>
    /// An implementation of the <see cref="ILogger"/> interface for entity framework
    /// </summary>
    public class LoggingAdapter : ILogger {
        /// <summary>
        /// The minimum entity framework log level
        /// </summary>
        public LogLevel MinLogLevel { get; }


        /// <summary>
        /// Create a new instance of <see cref="LoggingAdapter"/>
        /// </summary>
        public LoggingAdapter(LogLevel minLogLevel = LogLevel.Information) {
            MinLogLevel = minLogLevel;
        }


        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            var logLevels = ConvertLogLevel(logLevel);
            if((eventId.Id == 10403) || (Core.Diagnostics.Log.LogLevel > logLevels))
                return;

            if(formatter == null)
                return;

            var message = formatter(state, exception);
            if(string.IsNullOrEmpty(message) && (exception == null))
                return;

            if(exception != null)
                message += "\n" + exception.StackTrace;

            Core.Diagnostics.Log.Custom(logLevels, "EntityFramework Internal", nameof(LoggingAdapter), "EntityFramework", message);
        }

        bool ILogger.IsEnabled(LogLevel logLevel) {
            return logLevel >= MinLogLevel;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) {
            return null;
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
    }
}
