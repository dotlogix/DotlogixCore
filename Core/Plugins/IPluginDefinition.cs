// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IPluginDefinition.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
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
