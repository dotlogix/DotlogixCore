// ==================================================
// Copyright 2018(C) , DotLogix
// File:  GetterDelegate.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Reflection.Delegates {
    /// <summary>
    /// A delegate to represent getters
    /// </summary>
    public delegate object GetterDelegate(object instance);

    /// <summary>
    /// A delegate to represent getters
    /// </summary>
    public delegate TProperty GetterDelegate<in TInstance, out TProperty>(TInstance instance);
}
