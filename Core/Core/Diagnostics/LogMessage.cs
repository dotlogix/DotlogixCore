// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LogMessage.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// A log message
    /// </summary>
    public class LogMessage {
        private DateTime _utcTimeStamp;

        /// <summary>
        /// The timestamp in utc time
        /// </summary>
        public DateTime UtcTimeStamp {
            get => _utcTimeStamp;
            set {
                if(value.Kind != DateTimeKind.Utc)
                    value = value.ToUniversalTime();
                _utcTimeStamp = value;
            }
        }

        /// <summary>
        /// The timestamp in local time
        /// </summary>
        public DateTime TimeStamp => UtcTimeStamp.ToLocalTime();

        /// <summary>
        /// The log level
        /// </summary>
        public LogLevels LogLevel { get; set; }

        /// <summary>
        /// The log source of this message
        /// </summary>
        public ILogSource Source { get; set; }

        /// <summary>
        /// The thread captured while creating this message
        /// </summary>
        public string ThreadName { get; set; }

        /// <summary>
        /// The method name captured while creating this message
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// The type name captured while creating this message
        /// </summary>
        public string TypeName { get; set; }
        
        /// <summary>
        /// The message text
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The exception responsible for the current log message
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Additional properties of this message
        /// </summary>
        public IReadOnlyDictionary<string, object> Context { get; set; }
    }
}