// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LogMessage.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
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
        /// The local timestamp
        /// </summary>
        public DateTime TimeStamp => UtcTimeStamp.ToLocalTime();
        /// <summary>
        /// The log level
        /// </summary>
        public LogLevels LogLevel { get; set; }
        /// <summary>
        /// The calling method
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// The thread name
        /// </summary>
        public string ThreadName { get; set; }
        /// <summary>
        /// The class name
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; set; }
    }
}
