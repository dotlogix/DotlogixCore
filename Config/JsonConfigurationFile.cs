﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonConfigurationFile.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.IO;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Nodes.Processor;
#endregion

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
            } catch(Exception e) {
                Log.Error(e);
                return false;
            }
        }

        public override bool TrySave() {
            var autoReload = AutoReload;
            var enableFileWatching = EnableFileWatching;
            AutoReload = false;
            EnableFileWatching = false;
            try {
                var json = JsonNodes.ToJson(CurrentConfig, JsonFormatterSettings.Idented);
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
