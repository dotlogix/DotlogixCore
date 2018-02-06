// ==================================================
// Copyright 2016(C) , DotLogix
// File:  SetterDelegate.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

namespace DotLogix.Core.Reflection.Delegates {
    public delegate void SetterDelegate(object instance, object value);

    public delegate void SetterDelegate<in TInstance, in TProperty>(TInstance instance, TProperty value);
}
