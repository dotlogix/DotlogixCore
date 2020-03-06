// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LoggingAdapterProvider.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using Microsoft.Extensions.Logging;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Diagnostics {
    /// <summary>
    /// An implementation of the <see cref="ILoggerProvider"/> interface for entity framework
    /// </summary>
    public class LoggingAdapterProvider : ILoggerProvider {
        /// <summary>
        /// The minimum entity framework log level
        /// </summary>
        public LogLevel MinLogLevel { get; }

        /// <summary>
        /// Create a new instance of <see cref="LoggingAdapterProvider"/>
        /// </summary>
        public LoggingAdapterProvider(LogLevel minLogLevel = LogLevel.Information) {
            MinLogLevel = minLogLevel;
        }

        /// <inheritdoc />
        public void Dispose() { }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName) {
            return new LoggingAdapter(MinLogLevel);
        }
    }
}
