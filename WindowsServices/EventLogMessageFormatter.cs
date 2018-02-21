using System.Text;
using DotLogix.Core.Diagnostics;

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