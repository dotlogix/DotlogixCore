// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ILogMessageFormatter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Diagnostics {
    public interface ILogMessageFormatter {
        string Format(LogMessage logMessage);
    }
}
