using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Diagnostics; 

/// <summary>
/// An interface to broadcast log messages to multiple targets
/// </summary>
public interface IBroadcastLogger : ILogger {
    /// <summary>
    /// Adds a logger callback
    /// </summary>
    bool Subscribe(Action<LogMessage> callback);

    /// <summary>
    /// Adds an async logger callback
    /// </summary>
    bool Subscribe(Func<LogMessage, ValueTask> callback);

    /// <summary>
    /// Adds a logger instance
    /// </summary>
    bool Subscribe(ILogTarget logTarget);

    /// <summary>
    /// Adds an async logger instance
    /// </summary>
    bool Subscribe(IAsyncLogTarget logTarget);

    /// <summary>
    /// Removes a logger callback
    /// </summary>
    bool Unsubscribe(Action<LogMessage> callback);

    /// <summary>
    /// Adds an async logger callback
    /// </summary>
    bool Unsubscribe(Func<LogMessage, ValueTask> callback);

    /// <summary>
    /// Removes a logger instance
    /// </summary>
    bool Unsubscribe(ILogTarget logTarget);

    /// <summary>
    /// Removes an async logger instance
    /// </summary>
    bool Unsubscribe(IAsyncLogTarget logTarget);
}