// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EventLogLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Diagnostics;
using DotLogix.Core.Diagnostics;
#endregion

namespace DotLogix.Core.WindowsServices {
    public class EventLogLogger : LoggerBase {
        private static readonly ILogMessageFormatter MessageFormatter = new EventLogMessageFormatter();

        private readonly EventLog _eventLog;

        public EventLogLogger(EventLog eventLog) : base("EventLogLogger") {
            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
        }

        public override bool Log(LogMessage message) {
            switch(message.LogLevel) {
                case LogLevels.Trace:
                case LogLevels.Debug:
                case LogLevels.Info:
                    _eventLog.WriteEntry(MessageFormatter.Format(message), EventLogEntryType.Information);
                    break;
                case LogLevels.Warning:
                    _eventLog.WriteEntry(MessageFormatter.Format(message), EventLogEntryType.Warning);
                    break;
                case LogLevels.Error:
                case LogLevels.Critical:
                    _eventLog.WriteEntry(MessageFormatter.Format(message), EventLogEntryType.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }
    }
}
