// ==================================================
// Copyright 2016(C) , DotLogix
// File:  AccessorTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    [Flags]
    public enum AccessorTypes {
        None = 0,
        Property = 1 << 0,
        Field = 1 << 1,
        Any = Property | Field
    }
}
