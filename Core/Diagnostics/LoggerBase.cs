// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LoggerBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Diagnostics {
    /// <inheritdoc />
    public class LoggerBase : ILogger {
        /// <inheritdoc />
        public LoggerBase(string name) {
            Name = name;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public virtual bool Initialize() {
            return true;
        }

        /// <inheritdoc />
        public virtual bool Shutdown() {
            return true;
        }

        /// <inheritdoc />
        public virtual bool Log(LogMessage message) {
            return true;
        }
    }
}
