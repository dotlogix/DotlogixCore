// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LogMessage.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Diagnostics {
    public class LogMessage {
        private DateTime _utcTimeStamp;

        public DateTime UtcTimeStamp {
            get => _utcTimeStamp;
            set {
                if(value.Kind != DateTimeKind.Utc)
                    value = value.ToUniversalTime();
                _utcTimeStamp = value;
            }
        }

        public DateTime TimeStamp => UtcTimeStamp.ToLocalTime();
        public LogLevels LogLevel { get; set; }
        public string MethodName { get; set; }
        public string ThreadName { get; set; }
        public string ClassName { get; set; }
        public string Message { get; set; }
    }
}
