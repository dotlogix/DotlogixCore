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
using System.Text;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A console formatter
    /// </summary>
    public class LogMessageFormatter : ILogMessageFormatter {
        private readonly object _lock = new();
        private readonly StringBuilder _stringBuilder = new();

        /// <inheritdoc />
        public bool FormatTo(TextWriter writer, LogMessage message) {
            writer.Write(Format(message));
            return true;
        }

        /// <inheritdoc />
        public string Format(LogMessage message) {
            lock(_lock) {
                _stringBuilder.AppendLine($"{message.TimeStamp:G} - {message.LogLevel:G}, Type: {message.TypeName}, Method: {message.MethodName}, Thread: {message.ThreadName}");
            
                if(message.Context is { Count: > 0 }) {
                    _stringBuilder.AppendLine("Context:");
                    foreach (var (key, value) in message.Context.OrderBy(kv => kv.Key)) {
                        _stringBuilder.Append("  - ").Append(key).Append(": ").Append(value).AppendLine();
                    }
                }
            
                if(message.Message is not null) {
                    _stringBuilder.AppendLine(message.Message);
                }
                _stringBuilder.AppendLine();
            
                var messageText = _stringBuilder.ToString();
                _stringBuilder.Clear();
                return messageText;
            }
        }
    }
}