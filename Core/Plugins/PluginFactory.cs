// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PluginFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion

namespace DotLogix.Core.Plugins {
    public static class PluginFactory {
        public static IEnumerable<T> LoadPlugins<T>(string pluginPath, string filter = "*.dll",
                                                    SearchOption searchOption = SearchOption.TopDirectoryOnly)
            where T : IPluginDefinition {
            var files = Directory.GetFiles(pluginPath, filter, searchOption);
            return LoadPlugins<T>(files);
        }

        public static IEnumerable<T> LoadPlugins<T>(IEnumerable<string> files)
            where T : IPluginDefinition {
            var assemblies = LoadPluginAsseblies<T>(files);
            return assemblies.SelectMany(a => a.Instances);
        }

        public static IEnumerable<PluginAssembly<T>> LoadPluginAsseblies<T>(IEnumerable<string> files,
                                                                            bool validOnly = true)
            where T : IPluginDefinition {
            foreach(var file in files) {
                var assembly = new PluginAssembly<T>(file);
                if(assembly.Load() || (validOnly == false))
                    yield return assembly;
            }
        }
    }
}
