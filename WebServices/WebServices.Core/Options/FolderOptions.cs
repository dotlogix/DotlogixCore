// ==================================================
// Copyright 2019(C) , DotLogix
// File:  FolderConfiguration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  12.01.2019
// LastEdited:  20.01.2019
// ==================================================

namespace DotLogix.WebServices.Core.Options; 

public class FolderOptions {
    public string LogDirectory { get; set; } = "log";
    public string DataDirectory { get; set; } = "data";
    public string TempDirectory { get; set; } = "temp";
    public string RootDirectory { get; set; }
}