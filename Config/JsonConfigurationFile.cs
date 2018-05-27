﻿using System;
using System.IO;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;

namespace DotLogix.Core.Config {
    public class JsonConfigurationFile<TConfig> : ConfigurationFileBase<TConfig> where TConfig : class, new() {
        public JsonConfigurationFile(string fileName, string directory, bool autoReload) : base(fileName, directory, autoReload) { }
        public override bool TryLoad() {
            if(File.Exists(AbsolutePath) == false) {
                Log.Debug($"Configuration file for {typeof(TConfig).GetFriendlyName()} can not be found");
                return false;
            }

            try {
                var json = File.ReadAllText(AbsolutePath);
                CurrentConfig = JsonNodes.FromJson<TConfig>(json);
                return true;
            } catch (Exception e) {
                Log.Error(e);
                return false;
            }
        }

        public override bool TrySave() {
            var autoReload = AutoReload;
            var enableFileWatching = EnableFileWatching;
            AutoReload = false;
            EnableFileWatching = false;
            try
            {
                var json = JsonNodes.ToJson(CurrentConfig);
                File.WriteAllText(AbsolutePath, json);
                HasChanged = false;
                return true;
            } catch(Exception e) {
                Log.Error(e);
                return false;
            } finally {
                AutoReload = autoReload;
                EnableFileWatching = enableFileWatching;
            }
        }
    }
}