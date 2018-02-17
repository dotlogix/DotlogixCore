// ==================================================
// Copyright 2018(C) , DotLogix
// File:  GetterDelegate.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Reflection.Delegates {
    public delegate object GetterDelegate(object instance);

    public delegate TProperty GetterDelegate<in TInstance, out TProperty>(TInstance instance);
}
