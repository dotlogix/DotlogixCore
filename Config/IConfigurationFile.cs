// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IConfigurationFile.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.04.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Config {
    public interface IConfigurationFile<TConfig> {
        string FileName { get; }
        string Directory { get; }
        string AbsolutePath { get; }
        TConfig CurrentConfig { get; }
        bool TryLoad();
        bool TrySave();
    }
}
