// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  ConfigurationFileBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 22.08.2020 13:51
// LastEdited:  26.09.2021 22:27
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
    public abstract class ConfigurationFileBase<TConfig> : IConfigurationFile<TConfig>, IDisposable where TConfig : class, new() {
        private readonly FileSystemWatcher _watcher;

        private TConfig _currentConfig;
        /// <summary>
        /// An event to receive a notification as soon as the file system detects a change in the config file
        /// </summary>
        public event EventHandler<ConfigChangedEventArgs<TConfig>> ConfigChanged;

        /// <summary>
        /// An event to receive a notification as soon as the config file loaded the current file
        /// </summary>
        public event EventHandler<ConfigChangedEventArgs<TConfig>> ConfigLoaded;

        /// <summary>
        /// An event to receive a notification as soon as the config file failed to load the current file
        /// </summary>
        public event EventHandler<ConfigChangedEventArgs<TConfig>> ConfigError;

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
            HasChanged = true;

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
                HasChanged = false;

                var previous = _currentConfig;
                var success = value != null;
                _currentConfig = value ?? previous;

                OnConfigChanged(_currentConfig, previous, success);
            }
            get {
                if(HasChanged && (AutoReload || (_currentConfig == null))) {
                    TryLoad();
                }
                return _currentConfig;
            }
        }

        /// <summary>
        /// Emits change events based on the method inputs
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <param name="success"></param>
        protected void OnConfigChanged(TConfig current, TConfig previous, bool success) {
            var changedEventArgs = new ConfigChangedEventArgs<TConfig>(this, current, previous, success);
            if(success) {
                ConfigLoaded?.Invoke(this, changedEventArgs);
            } else {
                ConfigError?.Invoke(this, changedEventArgs);
            }
            ConfigChanged?.Invoke(this, changedEventArgs);
        }

        /// <inheritdoc />
        public abstract bool TryLoad();
        /// <inheritdoc />
        public abstract bool TrySave();

        private void _watcher_Changed(object sender, FileSystemEventArgs e) {
            HasChanged = true;
            if(AutoReload) {
                TryLoad();
            }
        }

        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                _watcher?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
