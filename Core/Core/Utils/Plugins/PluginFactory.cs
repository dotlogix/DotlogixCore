// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PluginFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion

namespace DotLogix.Core.Utils.Plugins {
    /// <summary>
    /// A factory implementation to load plugins
    /// </summary>
    public static class PluginFactory {
        /// <summary>
        /// Load all matching plugin types and create a new instance of the matching types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LoadPlugins<T>(string pluginPath, string filter = "*.dll",
                                                    SearchOption searchOption = SearchOption.TopDirectoryOnly)
            where T : IPluginDefinition {
            var files = Directory.GetFiles(pluginPath, filter, searchOption);
            return LoadPlugins<T>(files);
        }

        /// <summary>
        /// Load all matching plugin types and create a new instance of the matching types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LoadPlugins<T>(IEnumerable<string> files)
            where T : IPluginDefinition {
            var assemblies = LoadPluginAssemblies<T>(files);
            return assemblies.SelectMany(a => a.Instances);
        }

        /// <summary>
        /// Load all matching files as plugin assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<PluginAssembly<T>> LoadPluginAssemblies<T>(IEnumerable<string> files,
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
