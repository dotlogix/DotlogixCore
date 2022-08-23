// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicMemberTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics; 

/// <summary>
/// Types of members
/// </summary>
[Flags]
public enum DynamicMemberTypes {
    /// <summary>
    /// None
    /// </summary>
    None = 0,
    /// <summary>
    /// Constructors
    /// </summary>
    Constructor = 1 << 0,
    /// <summary>
    /// Events
    /// </summary>
    Event = 1 << 1,
    /// <summary>
    /// Fields
    /// </summary>
    Field = 1 << 2,
    /// <summary>
    /// Methods
    /// </summary>
    Method = 1 << 3,
    /// <summary>
    /// Properties
    /// </summary>
    Property = 1 << 4,
    /// <summary>
    /// Types
    /// </summary>
    Type = 1 << 5,
    /// <summary>
    /// Nested types
    /// </summary>
    NestedType = 1 << 6,
    /// <summary>
    /// Accessors
    /// </summary>
    Accessor = Field | Property,
    /// <summary>
    /// Any
    /// </summary>
    Any = Constructor | Event | Field | Method | Property | Type | NestedType
}