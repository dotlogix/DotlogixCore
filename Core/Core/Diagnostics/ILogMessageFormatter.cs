// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ILogMessageFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

using System.IO;

namespace DotLogix.Core.Diagnostics; 

/// <summary>
/// An interface for log message formatters
/// </summary>
public interface ILogMessageFormatter {
    /// <summary>
    /// Formats a log message
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="logMessage"></param>
    /// <returns></returns>
    bool FormatTo(TextWriter writer, LogMessage logMessage);
        
    /// <summary>
    /// Formats a log message
    /// </summary>
    /// <param name="logMessage"></param>
    /// <returns></returns>
    string Format(LogMessage logMessage);
}