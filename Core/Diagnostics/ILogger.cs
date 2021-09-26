// ==================================================
// Copyright 2014-2021(C) , DotLogix
// File:  ILogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:14
// ==================================================

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// An interface to receive log messages
    /// </summary>
    public interface ILogger {
        /// <summary>
        /// The name of the logger
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Initialize the logger instance
        /// </summary>
        /// <returns></returns>
        bool Initialize();
        /// <summary>
        /// Shutdown the logger instance
        /// </summary>
        /// <returns></returns>
        bool Shutdown();

        /// <summary>
        /// Callback to receive messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Log(LogMessage message);
    }
}
