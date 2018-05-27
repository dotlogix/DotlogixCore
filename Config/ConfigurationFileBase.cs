using System;
using System.IO;

namespace DotLogix.Core.Config
{
    public abstract class ConfigurationFileBase<TConfig> : IConfigurationFile<TConfig> where TConfig : class, new() {
        public EventHandler ConfigChanged;

        private readonly FileSystemWatcher _watcher;

        public string FileName { get; }
        public string Directory { get; }
        public bool AutoReload { get; set; }
        public string AbsolutePath { get; }
        public bool HasChanged { get; protected set; }

        private TConfig _currentConfig;

        public TConfig CurrentConfig {
            set {
                _currentConfig = value;
                HasChanged = false;
            }
            get {
                if(AutoReload && (HasChanged || _currentConfig == null))
                    TryLoad();
                return _currentConfig;
            }
        }

        public bool EnableFileWatching {
            get => _watcher.EnableRaisingEvents;
            set => _watcher.EnableRaisingEvents = value;
        }

        public ConfigurationFileBase(string fileName, string directory, bool autoReload) {
            FileName = fileName;
            Directory = directory;
            AutoReload = autoReload;
            AbsolutePath = Path.Combine(directory, fileName);

            _watcher = new FileSystemWatcher(directory, fileName)
                       {
                           EnableRaisingEvents = true,
                           NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                       };
            _watcher.Changed += _watcher_Changed;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e) {
            HasChanged = true;
            ConfigChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract bool TryLoad();
        public abstract bool TrySave();
    }
}
