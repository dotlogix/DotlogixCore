// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DynamicMemberTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    [Flags]
    public enum DynamicMemberTypes {
        None = 0,
        Constructor = 1 << 0,
        Event = 1 << 1,
        Field = 1 << 2,
        Method = 1 << 3,
        Property = 1 << 4,
        Type = 1 << 5,
        NestedType = 1 << 6,
        Accessor = Field | Property,
        Any = Constructor | Event | Field | Method | Property | Type | NestedType
    }
}
