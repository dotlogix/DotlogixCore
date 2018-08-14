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
    public class LoggingAdapterProvider : ILoggerProvider {
        public LogLevel MinLogLevel { get; }

        public LoggingAdapterProvider(LogLevel minLogLevel = LogLevel.Information) {
            MinLogLevel = minLogLevel;
        }

        public void Dispose() { }

        public ILogger CreateLogger(string categoryName) {
            return new LoggingAdapter(MinLogLevel);
        }
    }
}
