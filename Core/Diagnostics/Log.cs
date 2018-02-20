// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Log.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Diagnostics {
    public static class Log {
        private static readonly ParrallelLogger Logger = ParrallelLogger.Instance;

        public static LogLevels LogLevel {
            get => Logger.CurrentLogLevel;
            set => Logger.CurrentLogLevel = value;
        }

        public static bool Initialize() {
            return Logger.Initialize();
        }

        public static bool AttachLoggers(params ILogger[] loggers) {
            return Logger.AttachLogger(loggers);
        }

        public static bool DetachLoggers(params ILogger[] loggers) {
            return Logger.DetachLogger(loggers);
        }

        public static bool Shutdown() {
            return Logger.Shutdown();
        }

        public static void Trace(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, message, 2);
            Logger.Log(logMessage);
        }

        public static void Trace(Func<string> messageFunc)
        {
            if (Logger.IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        public static void Debug(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Debug))
                return;
            var logMessage = CreateLogMessage(LogLevels.Debug, message, 2);
            Logger.Log(logMessage);
        }
        public static void Debug(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Debug))
                return;
            var logMessage = CreateLogMessage(LogLevels.Debug, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        public static void MethodEnter() {
            if(Logger.IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, null, 2);
            logMessage.Message = $"Enter Method \"{logMessage.MethodName}\"";
            Logger.Log(logMessage);
        }

        public static void MethodExit() {
            if(Logger.IsLoggingDisabled(LogLevels.Trace))
                return;
            var logMessage = CreateLogMessage(LogLevels.Trace, null, 2);
            logMessage.Message = $"Exit Method \"{logMessage.MethodName}\"";
            Logger.Log(logMessage);
        }

        public static void Info(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Info))
                return;
            var logMessage = CreateLogMessage(LogLevels.Info, message, 2);
            Logger.Log(logMessage);
        }
        public static void Info(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Info))
                return;
            var logMessage = CreateLogMessage(LogLevels.Info, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        public static void Warn(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Warning))
                return;
            var logMessage = CreateLogMessage(LogLevels.Warning, message, 2);
            Logger.Log(logMessage);
        }
        public static void Warn(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Warning))
                return;
            var logMessage = CreateLogMessage(LogLevels.Warning, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

        public static void Error(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Error))
                return;
            var logMessage = CreateLogMessage(LogLevels.Error, message, 2);
            Logger.Log(logMessage);
        }
        public static void Error(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Error))
                return;
            var logMessage = CreateLogMessage(LogLevels.Error, messageFunc.Invoke(), 2);
            Logger.Log(logMessage);
        }

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

        public static void Critical(string message) {
            if(Logger.IsLoggingDisabled(LogLevels.Critical))
                return;

            var logMessage = CreateLogMessage(LogLevels.Critical, message, 2);
            Logger.Log(logMessage);
        }
        public static void Critical(Func<string> messageFunc) {
            if(Logger.IsLoggingDisabled(LogLevels.Critical))
                return;

            var logMessage = CreateLogMessage(LogLevels.Critical, messageFunc.Invoke(), 2);
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
