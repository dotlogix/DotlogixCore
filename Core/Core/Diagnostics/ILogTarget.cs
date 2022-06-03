// ==================================================
// Copyright 2014-2021(C) , DotLogix
// File:  ILogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:14
// ==================================================

using System;

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// An interface to receive log messages
    /// </summary>
    public interface ILogTarget : IDisposable  {
        /// <summary>
        /// The name of the log target
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Writes a log entry
        /// </summary>
        void Log(LogMessage message);

        /// <summary>
        /// Clears all buffers for this log target and causes any buffered messages to be processed.
        /// </summary>
        void Flush();
    }
}
