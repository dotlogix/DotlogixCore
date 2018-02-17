// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SetterDelegate.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Reflection.Delegates {
    public delegate void SetterDelegate(object instance, object value);

    public delegate void SetterDelegate<in TInstance, in TProperty>(TInstance instance, TProperty value);
}
