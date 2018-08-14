// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EventLogMessageFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Text;
using DotLogix.Core.Diagnostics;
#endregion

namespace DotLogix.Core.WindowsServices {
    public class EventLogMessageFormatter : ILogMessageFormatter {
        public string Format(LogMessage message) {
            var sb = new StringBuilder();
            sb.Append($"{message.TimeStamp:HH:mm:ss}");
            sb.Append("LogLevel: ");
            sb.AppendLine(message.LogLevel.ToString());

            sb.Append("ClassName: ");
            sb.AppendLine(message.ClassName);

            sb.Append("ThreadName: ");
            sb.AppendLine(message.ThreadName);

            sb.AppendLine("Message: ");
            sb.AppendLine(message.Message);
            return sb.ToString();
        }
    }
}
