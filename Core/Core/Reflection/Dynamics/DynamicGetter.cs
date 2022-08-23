// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicGetter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Dynamics; 

/// <summary>
/// A representation of a getter
/// </summary>
public sealed class DynamicGetter {
    /// <summary>
    /// The access modifiers
    /// </summary>
    public AccessModifiers Access { get; }

    /// <summary>
    /// The visibility modifiers
    /// </summary>
    public VisibilityModifiers Visibility { get; }
    /// <summary>
    /// The getter delegate
    /// </summary>
    public GetterDelegate GetterDelegate { get; }

    internal DynamicGetter(AccessModifiers access, VisibilityModifiers visibility, GetterDelegate getterDelegate) {
        GetterDelegate = getterDelegate ?? throw new ArgumentNullException(nameof(getterDelegate));
        Access = access;
        Visibility = visibility;
    }

    /// <summary>
    /// Get the value<br></br>
    /// The field must be static
    /// </summary>
    /// <returns></returns>
    public object GetValue() {
        return GetValue(null);
    }

    /// <summary>
    /// Get the value
    /// </summary>
    /// <returns></returns>
    public object GetValue(object instance) {
        if((instance == null) && ((Access & AccessModifiers.Static) == 0))
            throw new ArgumentNullException(nameof(instance), "Can not read value without a new instance");

        return GetterDelegate.Invoke(instance);
    }
}