#region
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Diagnostics {
    public class LogSource : ILogSource {
        private readonly Func<ILogSource, LogLevels> _minLogLevelFunc;
        private readonly ILogger _logger;
        private int _skipFrames;

        /// <inheritdoc />
        public string Name { get; }

        public LogSource(string name, ILogger logger, int skipFrames, Func<ILogSource, LogLevels> minLogLevelFunc) {
            Name = name;
            _logger = logger;
            _skipFrames = skipFrames;
            _minLogLevelFunc = minLogLevelFunc;
        }
        
        public LogSource(string name, ILogger logger, Func<ILogSource, LogLevels> minLogLevelFunc) {
            Name = name;
            _logger = logger;
            _skipFrames = 2;
            _minLogLevelFunc = minLogLevelFunc;
        }

        /// <inheritdoc />
        public bool Log(LogMessage message) {
            if(IsEnabled(message.LogLevel) == false) {
                return false;
            }

            message.Source ??= this;
            _logger.Log(message);
            return true;
        }

        /// <inheritdoc />
        public void Trace(string message, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Trace) == false) {
                return;
            }
            var logMessage = CreateLogMessage(LogLevels.Trace, message, context);
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Trace(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            if(IsEnabled(LogLevels.Trace) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Trace, messageFunc.Invoke(), contextFunc?.Invoke());
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Debug(string message, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Debug) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Debug, message, context);
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Debug(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            if(IsEnabled(LogLevels.Debug) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Debug, messageFunc.Invoke(), contextFunc?.Invoke());
            Log(logMessage);
        }

        /// <inheritdoc />
        public void MethodEnter() {
            if(IsEnabled(LogLevels.Trace) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Trace, "Enter Method");
            Log(logMessage);
        }

        /// <inheritdoc />
        public void MethodExit() {
            if(IsEnabled(LogLevels.Trace) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Trace, "Exit Method");
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Info(string message, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Info) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Info, message, context);
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Info(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            if(IsEnabled(LogLevels.Info) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Info, messageFunc.Invoke(), contextFunc?.Invoke());
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Warn(string message, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Warning) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Warning, message, context);
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Warn(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            if(IsEnabled(LogLevels.Warning) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Warning, messageFunc.Invoke(), contextFunc?.Invoke());
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Warn(Exception exception, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Warning) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Warning, exception.ToString(), exception.TargetSite, exception, context);
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Error(string message, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Error) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Error, message, context);
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Error(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            if(IsEnabled(LogLevels.Error) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Error, messageFunc.Invoke(), contextFunc?.Invoke());
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Error(Exception exception, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Error) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Error, exception.ToString(), exception.TargetSite, exception, context);
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Critical(string message, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Critical) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Critical, message, context);
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Critical(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            if(IsEnabled(LogLevels.Critical) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Critical, messageFunc.Invoke(), contextFunc?.Invoke());
            Log(logMessage);
        }

        /// <inheritdoc />
        public void Critical(Exception exception, IReadOnlyDictionary<string, object> context = null) {
            if(IsEnabled(LogLevels.Critical) == false) {
                return;
            }

            var logMessage = CreateLogMessage(LogLevels.Critical, exception.ToString(), exception.TargetSite, exception, context);
            Log(logMessage);
        }

        public bool IsEnabled(LogLevels logLevel) {
            return _minLogLevelFunc.Invoke(this) <= logLevel;
        }

        private LogMessage CreateLogMessage(LogLevels logLevel, string message, IReadOnlyDictionary<string, object> context = null) {
            var frame = new StackFrame(_skipFrames);
            var methodBase = frame.GetMethod();
            return CreateLogMessage(logLevel, message, methodBase, context: context);
        }

        private LogMessage CreateLogMessage(LogLevels logLevel, string message, MethodBase methodBase, Exception exception = null, IReadOnlyDictionary<string, object> context = null) {
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
                Context = context,
                Source = this
            };

            return logMessage;
        }
    }

    public class LogSource<T> : ILogSource<T> {
        private readonly ILogSource _logSource;


        public LogSource(ILogSourceProvider logSourceProvider) {
            _logSource = logSourceProvider.Create(typeof(T), 3);
        }

        /// <inheritdoc />
        public string Name => _logSource.Name;

        /// <inheritdoc />
        public bool IsEnabled(LogLevels logLevel) {
            return _logSource.IsEnabled(logLevel);
        }

        /// <inheritdoc />
        public void Trace(string message, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Trace(message, context);
        }

        /// <inheritdoc />
        public void Trace(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            _logSource.Trace(messageFunc, contextFunc);
        }

        /// <inheritdoc />
        public void Debug(string message, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Debug(message, context);
        }

        /// <inheritdoc />
        public void Debug(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            _logSource.Debug(messageFunc, contextFunc);
        }

        /// <inheritdoc />
        public void MethodEnter() {
            _logSource.MethodEnter();
        }

        /// <inheritdoc />
        public void MethodExit() {
            _logSource.MethodExit();
        }

        /// <inheritdoc />
        public void Info(string message, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Info(message, context);
        }

        /// <inheritdoc />
        public void Info(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            _logSource.Info(messageFunc, contextFunc);
        }

        /// <inheritdoc />
        public void Warn(string message, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Warn(message, context);
        }

        /// <inheritdoc />
        public void Warn(Exception exception, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Warn(exception, context);
        }

        /// <inheritdoc />
        public void Warn(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            _logSource.Warn(messageFunc, contextFunc);
        }

        /// <inheritdoc />
        public void Error(string message, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Error(message, context);
        }

        /// <inheritdoc />
        public void Error(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            _logSource.Error(messageFunc, contextFunc);
        }

        /// <inheritdoc />
        public void Error(Exception exception, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Error(exception, context);
        }

        /// <inheritdoc />
        public void Critical(string message, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Critical(message, context);
        }

        /// <inheritdoc />
        public void Critical(Func<string> messageFunc, Func<IReadOnlyDictionary<string, object>> contextFunc = null) {
            _logSource.Critical(messageFunc, contextFunc);
        }

        /// <inheritdoc />
        public void Critical(Exception exception, IReadOnlyDictionary<string, object> context = null) {
            _logSource.Critical(exception, context);
        }

        /// <inheritdoc />
        public bool Log(LogMessage message) {
            return _logSource.Log(message);
        }
    }
}