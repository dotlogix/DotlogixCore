// ==================================================
// Copyright 2019(C) , DotLogix
// File:  LoggerConfiguration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  20.01.2019
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Diagnostics;
#endregion

namespace DotLogix.WebServices.Core.Options {
    public class LoggerOptions {
        public LogLevels LogLevel { get; set; } = LogLevels.Debug;
        public IDictionary<string, LogLevels> Sources { get; set; } = new Dictionary<string, LogLevels>();
    }
}
