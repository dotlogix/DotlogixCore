// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  IAsyncLogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 11.11.2021 20:49
// LastEdited:  11.11.2021 20:49
// ==================================================

using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// An interface to receive log messages asynchronously
    /// </summary>
    public interface IAsyncLogTarget : IAsyncDisposable {
        /// <summary>
        /// The name of the log target
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Writes a log entry asynchronously
        /// </summary>
        ValueTask LogAsync(LogMessage message);

        /// <summary>
        /// Clears all buffers for this logger and causes any buffered messages to be processed asynchronously.
        /// </summary>
        ValueTask FlushAsync();
    }
}
