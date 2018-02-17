// ==================================================
// Copyright 2018(C) , DotLogix
// File:  TextLogMessageFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
#endregion

#region
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Diagnostics {
    public class TextLogMessageFormatter : ILogMessageFormatter {
        public string Format(LogMessage message) {
            var timeStamp = $"{message.TimeStamp:HH:mm:ss}";
            var logLevel = message.LogLevel.ToString().SetLength(' ', 15);
            var context = message.ClassName.SetLength(' ', 15);
            var method = message.MethodName.SetLength(' ', 15);
            var thread = message.ThreadName.SetLength(' ', 15);

            var messageLines = message.Message.Split('\n');
            var formatted = $"{timeStamp}    {logLevel}    {context}    {method}    {thread}    ";
            if(messageLines.Length > 1) {
                var padding = "\n" + new string(' ', 88);
                formatted += string.Join(padding, messageLines) + "\n";
            } else
                formatted += messageLines[0] + "\n";

            return formatted;
        }
    }
}
