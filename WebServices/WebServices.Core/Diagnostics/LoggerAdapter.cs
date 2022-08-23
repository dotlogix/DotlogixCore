using System;
using System.Reflection;
using System.Threading;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DotLogix.WebServices.Core.Diagnostics; 

/// <summary>
/// An implementation of the <see cref="ILogger" /> interface for entity framework
/// </summary>
public class LoggerAdapter : ILogger {
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
        if(LogSource.IsEnabled(logLevels) == false)
            return;

        if(formatter == null)
            return;

        var message = formatter(state, exception);
        var logMessage = CreateLogMessage(logLevels, message, exception?.TargetSite, exception);
        LogSource.Log(logMessage);
    }

    bool ILogger.IsEnabled(LogLevel logLevel) {
        return LogSource.IsEnabled(ConvertLogLevel(logLevel));
    }

    /// <inheritdoc />
    IDisposable ILogger.BeginScope<TState>(TState state) {
        return null;
    }

    private LogMessage CreateLogMessage(LogLevels logLevel, string message, MethodBase methodBase, Exception exception = null) {
        var utcNow = DateTime.UtcNow;
        var thread = Thread.CurrentThread;

        var typeName = methodBase?.DeclaringType?.GetFriendlyName() ?? "Undefined";
        var methodName = methodBase?.Name ?? "Undefined";
        var threadName = thread.Name ?? (thread.IsThreadPoolThread ? $"PoolThread {thread.ManagedThreadId}" : $"Thread {thread.ManagedThreadId}");

        var logMessage = new LogMessage {
            LogLevel = logLevel,
            UtcTimeStamp = utcNow,
            MethodName = methodName,
            TypeName = typeName,
            ThreadName = threadName,
            Message = message,
            Exception = exception,
            Source = LogSource
        };
        return logMessage;
    }
        
    private static LogLevels ConvertLogLevel(LogLevel logLevel) {
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