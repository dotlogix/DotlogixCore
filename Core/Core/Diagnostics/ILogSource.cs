using System;
using System.Collections.Generic;

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// An interface to represent log sources
    /// </summary>
    public interface ILogSource {
        /// <summary>
        /// The name of the log source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Checks if the log source is enabled for the specified log level
        /// </summary>
        bool IsEnabled(LogLevels logLevel);
        
        /// <summary>
        /// Write a trace message
        /// </summary>
        void Trace(string message, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Write a trace message
        /// </summary>
        void Trace(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null);

        /// <summary>
        /// Write a debug message
        /// </summary>
        void Debug(string message, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Write a debug message
        /// </summary>
        void Debug(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null);

        /// <summary>
        /// Writes a method enter message
        /// </summary>
        void MethodEnter();

        /// <summary>
        /// Writes a method exit message
        /// </summary>
        void MethodExit();

        /// <summary>
        /// Writes an info message
        /// </summary>
        void Info(string message, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Writes an info message
        /// </summary>
        void Info(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null);

        /// <summary>
        /// Writes a warning message
        /// </summary>
        void Warn(string message, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Writes a warning message
        /// </summary>
        void Warn(Exception exception, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Writes a warning message
        /// </summary>
        void Warn(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null);

        /// <summary>
        /// Writes a error message
        /// </summary>
        void Error(string message, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Writes a error message
        /// </summary>
        void Error(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null);

        /// <summary>
        /// Writes a error message
        /// </summary>
        void Error(Exception exception, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Writes a critical message
        /// </summary>
        void Critical(string message, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Writes a critical message
        /// </summary>
        void Critical(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null);

        /// <summary>
        /// Writes a critical message
        /// </summary>
        void Critical(Exception exception, IReadOnlyDictionary<string, object> context = null);

        /// <summary>
        /// Writes a log entry
        /// </summary>
        bool Log(LogMessage message);
    }

    /// <inheritdoc />
// ReSharper disable once UnusedTypeParameter
    public interface ILogSource<T> : ILogSource {
    }
}