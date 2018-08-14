// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ILogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Diagnostics {
    public interface ILogger {
        string Name { get; }
        bool Initialize();
        bool Shutdown();
        bool Log(LogMessage message);
    }
}
