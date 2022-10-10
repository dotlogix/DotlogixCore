// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Log.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A static logging interface
    /// </summary>
    public static class Log {
        private static readonly ParrallelLogger Logger = ParrallelLogger.Instance;
        /// <summary>
        /// The level to log messages
        /// </summary>
        public static LogLevels LogLevel {
            get => Logger.CurrentLogLevel;
            set => Logger.CurrentLogLevel = value;
        }

        /// <summary>
        /// Initialize the log instance
        /// </summary>
        /// <returns></returns>
        public static bool Initialize() {
            return Logger.Initialize();
        }

        /// <summary>
        /// Attach loggers to the log instance
        /// </summary>
        /// <param name="loggers"></param>
        /// <returns></returns>
        public static bool AttachLoggers(params ILogger[] loggers) {
            return Logger.AttachLogger(loggers);
        }

        /// <summary>
        /// Attach loggers to the log instance
        /// </summary>
        /// <param name="loggers"></param>
        /// <returns></returns>
        public static bool AttachLoggers(IEnumerable<ILogger> loggers) {
            return Logger.AttachLogger(loggers);
        }

        /// <summary>
        /// Detach some loggers from the log instance
        /// </summary>
        /// <param name="loggers"></param>
        /// <returns></returns>
        public static bool DetachLoggers(params ILogger[] loggers) {
            return Logger.DetachLogger(loggers);
        }
        /// <summary>
        /// Detach some loggers from the log instance
        /// </summary>
        /// <param name="loggers"></param>
        /// <returns></returns>
        public static bool DetachLoggers(IEnumerable<ILogger> loggers) {
            return Logger.DetachLogger(loggers);
        }
        /// <summary>
        /// Shutdown the log instance
        /// </summary>
        /// <returns></returns>
        public static bool Shutdown() {
            return Logger.Shutdown();
        }

        /// <summary>
        /// Write a trace message to the loggers
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, message, 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Write a trace message to the loggers
        /// </summary>
        /// <param name="messageFunc"></param>
        public static void Trace(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Write a debug message to the loggers
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Debug))
                return;
            var logMessage = CreateLogMessage(LogLevels.Debug, message, 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Write a debug message to the loggers
        /// </summary>
        /// <param name="messageFunc"></param>
        public static void Debug(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Debug))
                return;
            var logMessage = CreateLogMessage(LogLevels.Debug, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a method enter message to the loggers
        /// </summary>
        public static void MethodEnter() {
            if(Logger.IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, null, 2);
            logMessage.Message = $"Enter Method \"{logMessage.MethodName}\"";
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a method exit message to the loggers
        /// </summary>
        public static void MethodExit() {
            if(Logger.IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, null, 2);
            logMessage.Message = $"Exit Method \"{logMessage.MethodName}\"";
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes an info message to the loggers
        /// </summary>
        public static void Info(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Info))
                return;
            var logMessage = CreateLogMessage(LogLevels.Info, message, 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes an info message to the loggers
        /// </summary>
        public static void Info(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Info))
                return;
            var logMessage = CreateLogMessage(LogLevels.Info, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a warning message to the loggers
        /// </summary>
        public static void Warn(string message) {
            if (Logger.IsLoggingDisabled(LogLevels.Warning))
                return;
            var logMessage = CreateLogMessage(LogLevels.Warning, message, 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a warning message to the loggers
        /// </summary>
        public static void Warn(Exception exception) {
            if (Logger.IsLoggingDisabled(LogLevels.Warning))
                return;

            if (exception is AggregateException ae) {
                foreach (var inner in ae.InnerExceptions)
                    Warn(inner);
            } else if (exception.InnerException != null)
                Warn(exception.InnerException);


            var message = exception.Message + "\n" + exception.StackTrace;

            var logMessage = CreateLogMessage(LogLevels.Warning, message, exception.TargetSite);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a warning message to the loggers
        /// </summary>
        public static void Warn(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Warning))
                return;
            var logMessage = CreateLogMessage(LogLevels.Warning, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a error message to the loggers
        /// </summary>
        public static void Error(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Error))
                return;
            var logMessage = CreateLogMessage(LogLevels.Error, message, 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a error message to the loggers
        /// </summary>
        public static void Error(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Error))
                return;
            var logMessage = CreateLogMessage(LogLevels.Error, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a error message to the loggers
        /// </summary>
        public static void Error(Exception exception) {
            if(Logger.IsLoggingDisabled(LogLevels.Error))
                return;

            if(exception is AggregateException ae) {
                foreach(var inner in ae.InnerExceptions)
                    Error(inner);
            } else if(exception.InnerException != null)
                Error(exception.InnerException);


            var message = exception.Message + "\n" + exception.StackTrace;

            var logMessage = CreateLogMessage(LogLevels.Error, message, exception.TargetSite);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a critical message to the loggers
        /// </summary>
        public static void Critical(Exception exception) {
            if(Logger.IsLoggingDisabled(LogLevels.Critical))
                return;

            if(exception is AggregateException ae) {
                foreach(var inner in ae.InnerExceptions)
                    Critical(inner);
            } else if(exception.InnerException != null)
                Critical(exception.InnerException);

            var message = exception.Message + "\n" + exception.StackTrace;

            var logMessage = CreateLogMessage(LogLevels.Critical, message,
                                              exception.TargetSite);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a critical message to the loggers
        /// </summary>
        public static void Critical(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Critical))
                return;

            var logMessage = CreateLogMessage(LogLevels.Critical, message, 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a critical message to the loggers
        /// </summary>
        public static void Critical(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Critical))
                return;

            var logMessage = CreateLogMessage(LogLevels.Critical, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a custom message to the loggers
        /// </summary>
        public static void Custom(LogLevels logLevel, string methodName, string className, string threadName, string message) {
            if(Logger.IsLoggingDisabled(logLevel))
                return;

            var logMessage = new LogMessage {
                                                LogLevel = logLevel,
                                                UtcTimeStamp = DateTime.UtcNow,
                                                MethodName = methodName,
                                                ClassName = className,
                                                ThreadName = threadName ?? "Undefined",
                                                Message = message
                                            };

            Logger.Log(logMessage);
        }

        /// <summary>
        /// Writes a custom message to the loggers
        /// </summary>
        public static void Custom(LogLevels logLevel, string methodName, string className, string threadName, Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(logLevel))
                return;

            var logMessage = new LogMessage {
                                                LogLevel = logLevel,
                                                UtcTimeStamp = DateTime.UtcNow,
                                                MethodName = methodName,
                                                ClassName = className,
                                                ThreadName = threadName ?? "Undefined",
                                                Message = messageFunc.Invoke()
                                            };

            Logger.Log(logMessage);
        }

        private static LogMessage CreateLogMessage(LogLevels logLevel, string message, int framesToSkip) {
            var frame = new StackFrame(framesToSkip);
            var methodBase = frame.GetMethod();
            return CreateLogMessage(logLevel, message, methodBase);
        }

        private static LogMessage CreateLogMessage(LogLevels logLevel, string message, MethodBase methodBase) {
            var type = methodBase.DeclaringType;
            var typename = type == null ? "Undefined" : type.GetFriendlyName();
            var logMessage = new LogMessage {
                                                LogLevel = logLevel,
                                                UtcTimeStamp = DateTime.UtcNow,
                                                MethodName = methodBase.Name,
                                                ClassName = typename,
                                                ThreadName = Thread.CurrentThread.Name ?? "Undefined",
                                                Message = message
                                            };

            return logMessage;
        }
    }
}
