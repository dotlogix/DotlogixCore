// ==================================================
// Copyright 2018(C) , DotLogix
// File:  VisibilityModifiers.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    [Flags]
    public enum VisibilityModifiers {
        None = 0,
        Private = 1 << 0,
        Protected = 1 << 1,
        Internal = 1 << 2,
        ProtectedInternal = Protected | Internal,
        Public = 1 << 3,
        NonPublic = Private | Protected | Internal,
        Any = NonPublic | Public
    }
}
