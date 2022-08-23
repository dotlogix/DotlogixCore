// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IOrdered.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Common.Features; 

/// <summary>
///     An interface to force an order value
/// </summary>
public interface IOrdered {
    /// <summary>
    ///     The order priority
    /// </summary>
    int Order { get; set; }
}