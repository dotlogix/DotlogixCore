// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LoggerBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
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
