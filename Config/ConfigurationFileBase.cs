// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ConfigurationFileBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
#endregion

namespace DotLogix.Core.Config {
    /// <summary>
    /// An implementation of the <see cref="IConfigurationFile{TConfig}"/> interface
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public abstract class ConfigurationFileBase<TConfig> : IConfigurationFile<TConfig> where TConfig : class, new() {
        private readonly FileSystemWatcher _watcher;

        private TConfig _currentConfig;
        /// <summary>
        /// An event to receive a notification as soon as the file system detects a change in the config file
        /// </summary>
        public EventHandler ConfigChanged;

        /// <summary>
        /// A flag to auto reload the config file on change
        /// </summary>
        public bool AutoReload { get; set; }
        /// <summary>
        /// A flag to check if the file has changed since the last load
        /// </summary>
        public bool HasChanged { get; protected set; }

        /// <summary>
        /// A flag to enable or disable file system watching
        /// </summary>
        public bool EnableFileWatching {
            get => _watcher.EnableRaisingEvents;
            set => _watcher.EnableRaisingEvents = value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ConfigurationFileBase{TConfig}"/>
        /// </summary>
        public ConfigurationFileBase(string fileName, string directory, bool autoReload) {
            FileName = fileName;
            Directory = directory;
            AutoReload = autoReload;
            AbsolutePath = Path.Combine(directory, fileName);

            _watcher = new FileSystemWatcher(directory, fileName) {
                                                                      EnableRaisingEvents = true,
                                                                      NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime
                                                                  };
            _watcher.Changed += _watcher_Changed;
        }


        /// <inheritdoc />
        public string FileName { get; }
        /// <inheritdoc />
        public string Directory { get; }
        /// <inheritdoc />
        public string AbsolutePath { get; }

        /// <inheritdoc />
        public TConfig CurrentConfig {
            set {
                _currentConfig = value;
                HasChanged = false;
            }
            get {
                if(AutoReload && (HasChanged || (_currentConfig == null)))
                    TryLoad();
                return _currentConfig;
            }
        }

        /// <inheritdoc />
        public abstract bool TryLoad();
        /// <inheritdoc />
        public abstract bool TrySave();

        private void _watcher_Changed(object sender, FileSystemEventArgs e) {
            HasChanged = true;
            ConfigChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
