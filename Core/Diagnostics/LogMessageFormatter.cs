// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ConsoleLogMessageFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using System.Linq;
using System.Text;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A console formatter
    /// </summary>
    public class LogMessageFormatter : ILogMessageFormatter {
        public static LogMessageFormatter Default { get; } = new LogMessageFormatter();

        private LogMessageFormatter() {
        }

        /// <inheritdoc />
        public bool Format(LogMessage message, TextWriter writer) {
            writer.WriteLine($"{message.TimeStamp:G} - {message.LogLevel:G}, Class: {message.ClassName}, Method: {message.MethodName}, Thread: {message.MethodName}");

            if(message.Context != null && message.Context.Count > 0) {
                writer.WriteLine("Context:");
                foreach (var kv in message.Context) {
                    writer.WriteLine($"  - {kv.Key}: {kv.Value}");
                }
            }

            if(message.Message != null) {
                writer.WriteLine(message.Message);
            }

            writer.WriteLine();

            return true;
        }
    }
}
