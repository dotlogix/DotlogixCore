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
    public abstract class ConfigurationFileBase<TConfig> : IConfigurationFile<TConfig> where TConfig : class, new() {
        private readonly FileSystemWatcher _watcher;

        private TConfig _currentConfig;
        public EventHandler ConfigChanged;
        public bool AutoReload { get; set; }
        public bool HasChanged { get; protected set; }

        public bool EnableFileWatching {
            get => _watcher.EnableRaisingEvents;
            set => _watcher.EnableRaisingEvents = value;
        }

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

        public string FileName { get; }
        public string Directory { get; }
        public string AbsolutePath { get; }

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

        public abstract bool TryLoad();
        public abstract bool TrySave();

        private void _watcher_Changed(object sender, FileSystemEventArgs e) {
            HasChanged = true;
            ConfigChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
