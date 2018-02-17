// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ILogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
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
