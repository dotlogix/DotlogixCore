﻿using DotLogix.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Diagnostics {
    public class LoggingAdapterProvider : ILoggerProvider {
        public void Dispose() { }

        public LoggingAdapterProvider(LogLevels minLogLevel = LogLevels.Info)
        {
            MinLogLevel = minLogLevel;
        }

        public LogLevels MinLogLevel { get; }

        public ILogger CreateLogger(string categoryName) {
            return new LoggingAdapter(MinLogLevel);
        }


    }
}