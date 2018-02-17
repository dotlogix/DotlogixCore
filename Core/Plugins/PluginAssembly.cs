// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PluginAssembly.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.IO;
using System.Linq;
using System.Reflection;
#endregion

namespace DotLogix.Core.Plugins {
    public class PluginAssembly<T> {
        public PluginState PluginState { get; private set; }
        public string File { get; }
        public string Directory { get; }
        public Assembly Assembly { get; private set; }
        public T[] Instances { get; private set; }
        public Exception LastError { get; private set; }

        public PluginAssembly(string file) {
            File = file;
            Directory = Path.GetDirectoryName(file);
        }

        public bool Load() {
            if(PluginState != PluginState.None)
                return PluginState == PluginState.Loaded;
            var pluginType = typeof(T);
            Assembly = Assembly.LoadFile(File);
            var types = Assembly.GetExportedTypes();
            var validType = types.Where(type => pluginType.IsAssignableFrom(type)).ToArray();
            var count = validType.Length;
            var instances = new T[validType.Length];
            if(count > 0) {
                try {
                    for(var i = 0; i < count; i++)
                        instances[i] = (T)Activator.CreateInstance(validType[i]);
                } catch(Exception e) {
                    LastError = e;
                    Instances = null;
                    PluginState = PluginState.Failed;
                    return false;
                }
            }
            Instances = instances;
            PluginState = PluginState.Loaded;
            return true;
        }
    }
}
