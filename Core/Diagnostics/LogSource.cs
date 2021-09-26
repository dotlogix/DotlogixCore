using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Diagnostics {
    public class LogSource : ILogSource {
        private readonly Func<string, LogLevels> _getLogLevel;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ILogger Logger { get; }

        /// <inheritdoc />
        public LogLevels LogLevel => _getLogLevel?.Invoke(Name) ?? Diagnostics.Log.LogLevel;

        public int StackFramesToSkip { get; set; } = 2;

        public LogSource(string name, ILogger logger, Func<string, LogLevels> getLogLevel = null) {
            _getLogLevel = getLogLevel;
            Name = name;
            Logger = logger;
        }

        #region LogSource

        
        /// <inheritdoc />
        public void Trace(string message) {
            if (IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, message, StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Trace(Func<string> messageFunc) {
            if (IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, messageFunc.Invoke(), StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Debug(string message) {
            if (IsLoggingDisabled(LogLevels.Debug))
                return;
            var logMessage = CreateLogMessage(LogLevels.Debug, message, StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Debug(Func<string> messageFunc) {
            if (IsLoggingDisabled(LogLevels.Debug))
                return;
            var logMessage = CreateLogMessage(LogLevels.Debug, messageFunc.Invoke(), StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void MethodEnter() {
            if (IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, "Enter Method", StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void MethodExit() {
            if (IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, "Exit Method", StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Info(string message) {
            if (IsLoggingDisabled(LogLevels.Info))
                return;
            var logMessage = CreateLogMessage(LogLevels.Info, message, StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Info(Func<string> messageFunc) {
            if (IsLoggingDisabled(LogLevels.Info))
                return;
            var logMessage = CreateLogMessage(LogLevels.Info, messageFunc.Invoke(), StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Warn(string message) {
            if (IsLoggingDisabled(LogLevels.Warning))
                return;
            var logMessage = CreateLogMessage(LogLevels.Warning, message, StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Warn(Exception exception) {
            if (IsLoggingDisabled(LogLevels.Warning))
                return;

            var logMessage = CreateLogMessage(LogLevels.Warning, exception.ToString(), exception.TargetSite, exception);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Warn(Func<string> messageFunc) {
            if (IsLoggingDisabled(LogLevels.Warning))
                return;
            var logMessage = CreateLogMessage(LogLevels.Warning, messageFunc.Invoke(), StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Error(string message) {
            if (IsLoggingDisabled(LogLevels.Error))
                return;
            var logMessage = CreateLogMessage(LogLevels.Error, message, StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Error(Func<string> messageFunc) {
            if (IsLoggingDisabled(LogLevels.Error))
                return;
            var logMessage = CreateLogMessage(LogLevels.Error, messageFunc.Invoke(), StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Error(Exception exception) {
            if (IsLoggingDisabled(LogLevels.Error))
                return;

            var logMessage = CreateLogMessage(LogLevels.Error, exception.ToString(), exception.TargetSite, exception);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Critical(Exception exception) {
            if (IsLoggingDisabled(LogLevels.Critical))
                return;

            var logMessage = CreateLogMessage(LogLevels.Critical, exception.ToString(), exception.TargetSite, exception);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Critical(string message) {
            if (IsLoggingDisabled(LogLevels.Critical))
                return;

            var logMessage = CreateLogMessage(LogLevels.Critical, message, StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Critical(Func<string> messageFunc) {
            if (IsLoggingDisabled(LogLevels.Critical))
                return;

            var logMessage = CreateLogMessage(LogLevels.Critical, messageFunc.Invoke(), StackFramesToSkip);
            Log(logMessage);
        }


        /// <inheritdoc />
        public void Custom(LogLevels logLevel, string methodName, string className, string threadName, string message, Exception exception = null) {
            if (IsLoggingDisabled(logLevel))
                return;

            var logMessage = new LogMessage {
                                            LogLevel = logLevel,
                                            UtcTimeStamp = DateTime.UtcNow,
                                            MethodName = methodName,
                                            ClassName = className,
                                            ThreadName = threadName ?? "Undefined",
                                            Message = message,
                                            Exception = exception,
                                            Source = this
                                            };

            Log(logMessage);
        }

        /// <inheritdoc />
        public void Custom(LogLevels logLevel, string methodName, string className, string threadName, Func<string> messageFunc, Exception exception = null) {
            if (IsLoggingDisabled(logLevel))
                return;

            var logMessage = new LogMessage {
                                            LogLevel = logLevel,
                                            UtcTimeStamp = DateTime.UtcNow,
                                            MethodName = methodName,
                                            ClassName = className,
                                            ThreadName = threadName ?? "Undefined",
                                            Message = messageFunc.Invoke(),
                                            Exception = exception,
                                            Source = this
                                            };

            Log(logMessage);
        }
        #endregion

        /// <inheritdoc />
        public bool Log(LogMessage message) {
            if (IsLoggingDisabled(message.LogLevel))
                return true;

            message.Source ??= this;
            Logger.Log(message);
            return true;
        }

        private LogMessage CreateLogMessage(LogLevels logLevel, string message, int framesToSkip) {
            var frame = new StackFrame(framesToSkip);
            var methodBase = frame.GetMethod();
            return CreateLogMessage(logLevel, message, methodBase);
        }

        private LogMessage CreateLogMessage(LogLevels logLevel, string message, MethodBase methodBase, Exception exception = null) {
            var type = methodBase.DeclaringType;
            var typename = type == null ? "Undefined" : type.GetFriendlyName();
            var logMessage = new LogMessage {
                                            LogLevel = logLevel,
                                            UtcTimeStamp = DateTime.UtcNow,
                                            MethodName = methodBase.Name,
                                            ClassName = typename,
                                            ThreadName = Thread.CurrentThread.Name ?? "Undefined",
                                            Message = message,
                                            Exception = exception,
                                            Source = this
                                            };

            return logMessage;
        }

        private bool IsLoggingDisabled(LogLevels logLevel) {
            return LogLevel > logLevel;
        }
    }
    public class LogSource<T> : ILogSource<T> {
        private readonly ILogSource _logSource;
        public LogSource(ILogSourceProvider provider) {
            _logSource = provider.Create(typeof(T).FullName, 3);
        }

        public string Name => _logSource.Name;

        public LogLevels LogLevel => _logSource.LogLevel;

        public ILogger Logger => _logSource.Logger;

        public void Trace(string message) {
            _logSource.Trace(message);
        }

        public void Trace(Func<string> messageFunc) {
            _logSource.Trace(messageFunc);
        }

        public void Debug(string message) {
            _logSource.Debug(message);
        }

        public void Debug(Func<string> messageFunc) {
            _logSource.Debug(messageFunc);
        }

        public void MethodEnter() {
            _logSource.MethodEnter();
        }

        public void MethodExit() {
            _logSource.MethodExit();
        }

        public void Info(string message) {
            _logSource.Info(message);
        }

        public void Info(Func<string> messageFunc) {
            _logSource.Info(messageFunc);
        }

        public void Warn(string message) {
            _logSource.Warn(message);
        }

        public void Warn(Exception exception) {
            _logSource.Warn(exception);
        }

        public void Warn(Func<string> messageFunc) {
            _logSource.Warn(messageFunc);
        }

        public void Error(string message) {
            _logSource.Error(message);
        }

        public void Error(Func<string> messageFunc) {
            _logSource.Error(messageFunc);
        }

        public void Error(Exception exception) {
            _logSource.Error(exception);
        }

        public void Critical(Exception exception) {
            _logSource.Critical(exception);
        }

        public void Critical(string message) {
            _logSource.Critical(message);
        }

        public void Critical(Func<string> messageFunc) {
            _logSource.Critical(messageFunc);
        }

        public void Custom(LogLevels logLevel, string methodName, string className, string threadName, string message, Exception exception = null) {
            _logSource.Custom(logLevel, methodName, className, threadName, message, exception);
        }

        public void Custom(LogLevels logLevel, string methodName, string className, string threadName, Func<string> messageFunc, Exception exception = null) {
            _logSource.Custom(logLevel, methodName, className, threadName, messageFunc, exception);
        }

        public bool Log(LogMessage message) {
            return _logSource.Log(message);
        }
    }
}