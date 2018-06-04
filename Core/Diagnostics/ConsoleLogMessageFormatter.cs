// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ConsoleLogMessageFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Linq;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Diagnostics {
    public class ConsoleLogMessageFormatter : ILogMessageFormatter {
        private static readonly string Padding =
        $"\n{new string(' ', 8)} | {new string(' ', 8)} | {new string(' ', 15)} | {new string(' ', 15)} | {new string(' ', 15)} | "
        ;

        private static readonly int PaddingLength = 76;

        public int ConsoleWidth { get; }

        public ConsoleLogMessageFormatter(int consoleWidth) {
            if(consoleWidth < 100)
                throw new ArgumentOutOfRangeException(nameof(consoleWidth), "ConsoleWidth must be greater than 100");
            ConsoleWidth = consoleWidth;
        }

        public string Format(LogMessage message) {
            var timeStamp = $"{message.TimeStamp:HH:mm:ss}";
            var logLevel = message.LogLevel.ToString().SetLength(' ', 8);
            var context = message.ClassName.SetLength(' ', 15);
            var method = message.MethodName.SetLength(' ', 15);
            var thread = message.ThreadName.SetLength(' ', 15);

            var messageLines = message.Message.WordWrap(ConsoleWidth - PaddingLength).ToList();
            var formatted = $"{timeStamp} | {logLevel} | {context} | {method} | {thread} | ";
            if(messageLines.Count > 1)
                formatted += string.Join(Padding, messageLines);
            else
                formatted += messageLines[0];

            return formatted;
        }
    }
}
