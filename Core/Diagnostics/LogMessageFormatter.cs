// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ConsoleLogMessageFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.IO;
using System.Linq;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A console formatter
    /// </summary>
    public class LogMessageFormatter : ILogMessageFormatter {
        /// <inheritdoc />
        public bool Write(TextWriter writer, LogMessage message) {
            writer.WriteLine($"{message.TimeStamp:G} - {message.LogLevel:G}, Class: {message.ClassName}, Method: {message.MethodName}, Thread: {message.ThreadName}");

            if(message.Context != null && message.Context.Count > 0) {
                writer.WriteLine("Context:");
                foreach (var kv in message.Context.OrderBy(kv => kv.Key)) {
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
