using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Diagnostics;

/// <summary>
/// An interface to receive log messages
/// </summary>
public interface ILogger : IDisposable, IAsyncDisposable {
    /// <summary>
    /// The name of the log target
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Writes a log entry
    /// </summary>
    void Log(LogMessage message);

    /// <summary>
    /// Clears all buffers for this logger and causes any buffered messages to be processed.
    /// </summary>
    void Flush();
}