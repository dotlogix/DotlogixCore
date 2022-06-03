// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CtorDelegate.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Reflection.Delegates {
    /// <summary>
    /// A delegate to represent constructors
    /// </summary>
    /// <param name="parameters"></param>
    public delegate object CtorDelegate(object[] parameters);
}
