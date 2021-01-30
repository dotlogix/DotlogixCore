// ==================================================
// Copyright 2018(C) , DotLogix
// File:  JsonConfigurationFile.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Nodes.Formats.Json;
#endregion

namespace DotLogix.Core.Config {
    /// <summary>
    ///     An implementation of the <see cref="IConfigurationFile{TConfig}" /> interface to load json files
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public class JsonConfigurationFile<TConfig> : ConfigurationFileBase<TConfig> where TConfig : class, new() {
        private const int MaxRetries = 5;
        private const int ReloadDelay = 1000;

        /// <summary>
        ///     Creates a new instance of <see cref="JsonConfigurationFile{TConfig}" />
        /// </summary>
        public JsonConfigurationFile(string fileName, string directory, bool autoReload) : base(fileName, directory, autoReload) { }

        /// <inheritdoc />
        public override bool TryLoad() {
            if(File.Exists(AbsolutePath) == false) {
                Log.Debug($"Configuration file for {typeof(TConfig).GetFriendlyName()} can not be found");
                return false;
            }

            var exceptions = new List<Exception>();
            for (var retry = 1; retry <= MaxRetries; retry++) {
                try {
                    var json = File.ReadAllText(AbsolutePath);
                    CurrentConfig = JsonUtils.FromJson<TConfig>(json);
                    Log.Debug($"Loading attempt {retry} config file \"{AbsolutePath}\" succeeded");
                    return true;
                } catch(IOException io) {
                    Log.Warn($"Loading attempt {retry} config file \"{AbsolutePath}\" failed ... retry in 1s");
                    Task.Delay(ReloadDelay).Wait();
                    exceptions.Add(io);
                } catch(Exception e) {
                    exceptions.Add(e);
                    break;
                }
            }

            CurrentConfig = null;
            Log.Error(new AggregateException(exceptions));
            return false;
        }

        /// <inheritdoc />
        public override bool TrySave() {
            var autoReload = AutoReload;
            var enableFileWatching = EnableFileWatching;
            AutoReload = false;
            EnableFileWatching = false;
            try {
                var json = JsonUtils.ToJson(CurrentConfig, JsonConverterSettings.Idented);
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
