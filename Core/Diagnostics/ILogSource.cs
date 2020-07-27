using System;

namespace DotLogix.Core.Diagnostics {
    public interface ILogSource : ILogger {
        /// <summary>
        /// The logger target
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Enables or disables this log source
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The parent logger source
        /// </summary>
        public ILogSource Parent { get; }

        /// <summary>
        /// The current log level
        /// </summary>
        LogLevels LogLevel { get; }

        /// <summary>
        /// Write a trace message
        /// </summary>
        /// <param name="message"></param>
        void Trace(string message);

        /// <summary>
        /// Write a trace message
        /// </summary>
        /// <param name="messageFunc"></param>
        void Trace(Func<string> messageFunc);

        /// <summary>
        /// Write a debug message
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// Write a debug message
        /// </summary>
        /// <param name="messageFunc"></param>
        void Debug(Func<string> messageFunc);

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
        void Info(string message);

        /// <summary>
        /// Writes an info message
        /// </summary>
        void Info(Func<string> messageFunc);

        /// <summary>
        /// Writes a warning message
        /// </summary>
        void Warn(string message);

        /// <summary>
        /// Writes a warning message
        /// </summary>
        void Warn(Exception exception);

        /// <summary>
        /// Writes a warning message
        /// </summary>
        void Warn(Func<string> messageFunc);

        /// <summary>
        /// Writes a error message
        /// </summary>
        void Error(string message);

        /// <summary>
        /// Writes a error message
        /// </summary>
        void Error(Func<string> messageFunc);

        /// <summary>
        /// Writes a error message
        /// </summary>
        void Error(Exception exception);

        /// <summary>
        /// Writes a critical message
        /// </summary>
        void Critical(Exception exception);

        /// <summary>
        /// Writes a critical message
        /// </summary>
        void Critical(string message);

        /// <summary>
        /// Writes a critical message
        /// </summary>
        void Critical(Func<string> messageFunc);
        
        /// <summary>
        /// Writes a custom message
        /// </summary>
        void Custom(LogLevels logLevel, string methodName, string className, string threadName, string message, Exception exception = null);

        /// <summary>
        /// Writes a custom message
        /// </summary>
        void Custom(LogLevels logLevel, string methodName, string className, string threadName, Func<string> messageFunc, Exception exception = null);
        
        /// <summary>
        /// Creates a nested logger source with an optional log level override
        /// </summary>
        public ILogSource CreateSource(string name, LogLevels? customLogLevel = null);

        /// <summary>
        /// Creates a nested logger source with an optional log level override
        /// </summary>
        public ILogSource CreateSource(string name, Func<LogLevels?> getCustomLogLevelFunc);

        /// <summary>
        /// Creates a nested logger source with an optional log level override
        /// </summary>
        public ILogSource CreateSource<T>(LogLevels? customLogLevel = null);

        /// <summary>
        /// Creates a nested logger source with an optional log level override
        /// </summary>
        public ILogSource CreateSource<T>(Func<LogLevels?> getCustomLogLevelFunc);
    }
}