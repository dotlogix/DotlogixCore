// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IConfigurationFile.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.04.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Config {
    /// <summary>
    /// An interface to represent configuration files
    /// </summary>
    public interface IConfigurationFile<out TConfig> {
        /// <summary>
        /// The file name
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// The parent directory
        /// </summary>
        string Directory { get; }
        /// <summary>
        /// The absolute path of the config file
        /// </summary>
        string AbsolutePath { get; }
        /// <summary>
        /// The current configuration instance
        /// </summary>
        TConfig CurrentConfig { get; }
        /// <summary>
        /// Tries to load the config file
        /// </summary>
        /// <returns></returns>
        bool TryLoad();
        /// <summary>
        /// Tries to save the config file
        /// </summary>
        /// <returns></returns>
        bool TrySave();
    }
}
