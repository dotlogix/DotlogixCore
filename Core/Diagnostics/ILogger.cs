// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ILogger.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
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
