// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PluginAssembly.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Extensions;
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

        public bool Load(params object[] args) {
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
                        instances[i] = validType[i].Instantiate<T>(args);
                } catch(Exception e) {
                    if(LastError != null) {
                        var exceptions = LastError is AggregateException ae ? ae.InnerExceptions.ToList() : new List<Exception> {LastError};
                        exceptions.Add(e);
                        LastError = new AggregateException(exceptions);
                    }


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
