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
    /// <summary>
    /// A logging adapter to log to the windows event log
    /// </summary>
    public class EventLogLogger : LoggerBase {
        private static readonly ILogMessageFormatter MessageFormatter = new EventLogMessageFormatter();

        private readonly EventLog _eventLog;

        /// <summary>
        /// Creates a new instance of <see cref="EventLogLogger"/>
        /// </summary>
        public EventLogLogger(EventLog eventLog) : base("EventLogLogger") {
            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
        }


        /// <inheritdoc />
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
