// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LogLevels.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Diagnostics {
    [Flags]
    public enum LogLevels {
        Trace = 1,
        Debug = 2,
        Info = 3,
        Warning = 4,
        Error = 5,
        Critical = 6,
        Off = 7
    }
}
