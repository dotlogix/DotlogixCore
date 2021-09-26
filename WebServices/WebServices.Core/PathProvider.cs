// ==================================================
// Copyright 2019(C) , DotLogix
// File:  PathConstants.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  16.07.2018
// LastEdited:  20.01.2019
// ==================================================

#region
 using System.IO;
 using System.Reflection;
 using DotLogix.WebServices.Core.Options;
 using Microsoft.Extensions.Options;
#endregion

namespace DotLogix.WebServices.Core {
    public class PathProvider : IPathProvider {
        private readonly IOptions<FolderOptions> _folderOptions;
        private FolderOptions FolderOptions => _folderOptions.Value;
        
        public string BinDirectory { get; }
        public string LogDirectory => ToAbsolutePath(FolderOptions?.LogDirectory);

        public string DataDirectory => ToAbsolutePath(FolderOptions?.DataDirectory);
        public string TempDirectory => ToAbsolutePath(FolderOptions?.TempDirectory);
        public string RootDirectory => ToAbsolutePath(FolderOptions?.RootDirectory ?? BinDirectory);

        public PathProvider(IOptions<FolderOptions> folderOptions) {
            _folderOptions = folderOptions;
            BinDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        }

        public string ToAbsolutePath(params string[] paths) {
            return ToAbsolutePath(Path.Combine(paths));
        }

        private string ToAbsolutePath(string path1) {
            if(Path.IsPathRooted(path1) == false)
                path1 = Path.Combine(RootDirectory, path1);
            return path1;
        }

        public string ToAbsolutePath(string path1, string path2) {
            return ToAbsolutePath(Path.Combine(path1, path2));
        }

        public string ToAbsolutePath(string path1, string path2, string path3) {
            return ToAbsolutePath(Path.Combine(path1, path2, path3));
        }
        
        public string EnsureDirectory(string absolutePath) {
            if(Directory.Exists(absolutePath) == false)
                Directory.CreateDirectory(absolutePath);
            return absolutePath;
        }
    }
}
