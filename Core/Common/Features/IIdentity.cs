// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IIdentity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

namespace DotLogix.Common.Features; 

/// <summary>
///     An interface to force an id
/// </summary>
public interface IIdentity {
    /// <summary>
    ///     The id
    /// </summary>
    int Id { get; set; }
}