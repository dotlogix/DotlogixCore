// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ILogMessageFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

namespace DotLogix.Core.Diagnostics {
    public interface ILogMessageFormatter {
        string Format(LogMessage logMessage);
    }
}
