// ==================================================
// Copyright 2016(C) , DotLogix
// File:  IPluginDefinition.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Plugins {
    public interface IPluginDefinition {
        Guid Id { get; }
        string Name { get; }
    }
}
