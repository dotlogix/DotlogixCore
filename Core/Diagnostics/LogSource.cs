using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Diagnostics {
    public sealed class LogSource : ILogSource {
        private readonly Func<LogLevels> _getCustomLogLevelFunc;
        /// <inheritdoc />
        public string Name { get; }
        /// <inheritdoc />
        public ILogger Logger { get; }
        /// <inheritdoc />
        public ILogSource Parent { get; }

        /// <inheritdoc />
        public LogLevels LogLevel => _getCustomLogLevelFunc.Invoke();

        public int StackFramesToSkip { get; set; } = 2;

        /// <inheritdoc />
        private LogSource(string name, ILogSource parent, LogLevels? logLevel, ILogger target = null) : this(name, parent, logLevel.HasValue ? () => logLevel : default(Func<LogLevels?>), target) {
        }

        /// <inheritdoc />
        private LogSource(string name, ILogSource parent, Func<LogLevels?> getCustomLogLevelFunc = null, ILogger target = null) {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Logger = target ?? parent.Logger ?? throw new ArgumentNullException(nameof(target));

            if (getCustomLogLevelFunc != null) {
                _getCustomLogLevelFunc = () => {
                                             var currentLogLevel = getCustomLogLevelFunc.Invoke();
                                             if(currentLogLevel.HasValue)
                                                 return (LogLevels)Math.Max((int)Parent.LogLevel, (int)currentLogLevel.Value);
                                             return Parent.LogLevel;
                                         };
            } else {
                _getCustomLogLevelFunc = () => Parent.LogLevel;
            }
        }

        /// <inheritdoc />
        public LogSource(string name, ILogger target, LogLevels logLevel) : this(name, target, () => logLevel) {
        }

        /// <inheritdoc />
        public LogSource(string name, ILogger target, Func<LogLevels> getCustomLogLevelFunc) {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Logger = target ?? throw new ArgumentNullException(nameof(target));
            _getCustomLogLevelFunc = getCustomLogLevelFunc ?? throw new ArgumentNullException(nameof(getCustomLogLevelFunc));
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

        #region Logger

        bool ILogger.Initialize() => true;
        bool ILogger.Shutdown() => true;

        /// <inheritdoc />
        public bool Log(LogMessage message) {
            if (IsLoggingDisabled(message.LogLevel))
                return true;

            message.Source ??= this;
            Logger.Log(message);
            return true;
        }

        #endregion

        #region CreateSource
        /// <inheritdoc />
        public ILogSource CreateSource(string name, LogLevels? customLogLevel = null) {
            return new LogSource(name, this, customLogLevel);
        }

        /// <inheritdoc />
        public ILogSource CreateSource(string name, Func<LogLevels?> getCustomLogLevelFunc) {
            return new LogSource(name, this, getCustomLogLevelFunc);
        }

        /// <inheritdoc />
        public ILogSource CreateSource<T>(LogLevels? customLogLevel = null) {
            return new LogSource(typeof(T).Name, this, customLogLevel);
        }

        /// <inheritdoc />
        public ILogSource CreateSource<T>(Func<LogLevels?> getCustomLogLevelFunc) {
            return new LogSource(typeof(T).Name, this, getCustomLogLevelFunc);
        }

        #endregion

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
}