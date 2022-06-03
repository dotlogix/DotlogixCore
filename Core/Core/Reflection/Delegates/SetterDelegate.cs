// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SetterDelegate.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Reflection.Delegates {
    /// <summary>
    /// A delegate to represent setters
    /// </summary>

    public delegate void SetterDelegate(object instance, object value);

    /// <summary>
    /// A delegate to represent setters
    /// </summary>
    public delegate void SetterDelegate<in TInstance, in TProperty>(TInstance instance, TProperty value);
}
