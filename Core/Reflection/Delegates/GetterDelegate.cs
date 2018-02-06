// ==================================================
// Copyright 2016(C) , DotLogix
// File:  GetterDelegate.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

namespace DotLogix.Core.Reflection.Delegates {
    public delegate object GetterDelegate(object instance);

    public delegate TProperty GetterDelegate<in TInstance, out TProperty>(TInstance instance);
}
