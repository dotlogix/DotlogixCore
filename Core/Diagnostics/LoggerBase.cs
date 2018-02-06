// ==================================================
// Copyright 2016(C) , DotLogix
// File:  LoggerBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

namespace DotLogix.Core.Diagnostics {
    public class LoggerBase : ILogger {
        public LoggerBase(string name) {
            Name = name;
        }

        public string Name { get; }

        public virtual bool Initialize() {
            return true;
        }

        public virtual bool Shutdown() {
            return true;
        }

        public virtual bool Log(LogMessage message) {
            return true;
        }
    }
}
