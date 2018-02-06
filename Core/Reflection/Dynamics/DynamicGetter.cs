// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DynamicGetter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public sealed class DynamicGetter {
        public AccessModifiers Access { get; }
        public VisibilityModifiers Visibility { get; }
        public GetterDelegate GetterDelegate { get; }

        internal DynamicGetter(AccessModifiers access, VisibilityModifiers visibility, GetterDelegate getterDelegate) {
            GetterDelegate = getterDelegate ?? throw new ArgumentNullException(nameof(getterDelegate));
            Access = access;
            Visibility = visibility;
        }

        public object GetValue() {
            return GetValue(null);
        }

        public object GetValue(object instance) {
            if((instance == null) && ((Access & AccessModifiers.Static) == 0))
                throw new ArgumentNullException(nameof(instance), "Can not read value without an instance");

            return GetterDelegate.Invoke(instance);
        }
    }
}
