// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AccessModifiers.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    [Flags]
    public enum AccessModifiers {
        None = 0,
        Static = 1 << 1,
        Const = 1 << 2,
        Nested = 1 << 3,
        Abstract = 1 << 4,
        Virtual = 1 << 5,
        Sealed = 1 << 6
    }
}
