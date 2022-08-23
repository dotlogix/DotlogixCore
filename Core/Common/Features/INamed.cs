// ==================================================
// Copyright 2018(C) , DotLogix
// File:  INamed.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Common.Features; 

/// <summary>
///     An interface to force a name
/// </summary>
public interface INamed {
    /// <summary>
    ///     The name
    /// </summary>
    string Name { get; set; }
}