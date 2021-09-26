// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Log.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A static logging interface
    /// </summary>
    public static class Log {
        /// <summary>
        /// The global logger instance
        /// </summary>
        public static ILogSourceProvider LogSourceProvider { get; set; } = new LogSourceProvider(BroadcastLogger.Instance, _ => LogLevel);
        
        /// <summary>
        /// Gets or sets the default logger source (name = "Default")
        /// </summary>
        public static ILogSource Default => LogSourceProvider.Create("Default");
        
        /// <summary>
        /// Gets or sets the default log level
        /// </summary>
        public static LogLevels LogLevel { get; set; }

        /// <summary>
        /// Write a trace message to the loggers
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message) => Default.Trace(message);

        /// <summary>
        /// Write a trace message to the loggers
        /// </summary>
        /// <param name="messageFunc"></param>
        public static void Trace(Func<string> messageFunc) => Default.Trace(messageFunc);

        /// <summary>
        /// Write a debug message to the loggers
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message) => Default.Debug(message);

        /// <summary>
        /// Write a debug message to the loggers
        /// </summary>
        /// <param name="messageFunc"></param>
        public static void Debug(Func<string> messageFunc) => Default.Debug(messageFunc);

        /// <summary>
        /// Writes a method enter message to the loggers
        /// </summary>
        public static void MethodEnter() => Default.MethodEnter();

        /// <summary>
        /// Writes a method exit message to the loggers
        /// </summary>
        public static void MethodExit() => Default.MethodExit();

        /// <summary>
        /// Writes an info message to the loggers
        /// </summary>
        public static void Info(string message) => Default.Info(message);

        /// <summary>
        /// Writes an info message to the loggers
        /// </summary>
        public static void Info(Func<string> messageFunc) => Default.Info(messageFunc);

        /// <summary>
        /// Writes a warning message to the loggers
        /// </summary>
        public static void Warn(string message) => Default.Warn(message);

        /// <summary>
        /// Writes a warning message to the loggers
        /// </summary>
        public static void Warn(Exception exception) => Default.Warn(exception);

        /// <summary>
        /// Writes a warning message to the loggers
        /// </summary>
        public static void Warn(Func<string> messageFunc) => Default.Warn(messageFunc);

        /// <summary>
        /// Writes a error message to the loggers
        /// </summary>
        public static void Error(string message) => Default.Error(message);

        /// <summary>
        /// Writes a error message to the loggers
        /// </summary>
        public static void Error(Func<string> messageFunc) => Default.Error(messageFunc);

        /// <summary>
        /// Writes a error message to the loggers
        /// </summary>
        public static void Error(Exception exception) => Default.Error(exception);

        /// <summary>
        /// Writes a critical message to the loggers
        /// </summary>
        public static void Critical(Exception exception) => Default.Critical(exception);

        /// <summary>
        /// Writes a critical message to the loggers
        /// </summary>
        public static void Critical(string message) => Default.Critical(message);

        /// <summary>
        /// Writes a critical message to the loggers
        /// </summary>
        public static void Critical(Func<string> messageFunc) => Default.Critical(messageFunc);

        /// <summary>
        /// Writes a custom message to the loggers
        /// </summary>
        public static void Custom(LogLevels logLevel, string methodName, string className, string threadName, string message, Exception exception = null) {
            Default.Custom(logLevel, methodName, className, threadName, message, exception);
        }

        /// <summary>
        /// Writes a custom message to the loggers
        /// </summary>
        public static void Custom(LogLevels logLevel, string methodName, string className, string threadName, Func<string> messageFunc, Exception exception = null) {
            Default.Custom(logLevel, methodName, className, threadName, messageFunc, exception);
        }

        /// <summary>
        /// Creates a custom logger source
        /// </summary>
        public static ILogSource CreateSource(string name) {
            return LogSourceProvider.Create(name);
        }
    }
}
