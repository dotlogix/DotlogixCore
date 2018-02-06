// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DynamicSetter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    public sealed class DynamicSetter {
        public AccessModifiers Access { get; }
        public VisibilityModifiers Visibility { get; }
        public SetterDelegate SetterDelegate { get; }

        internal DynamicSetter(AccessModifiers access, VisibilityModifiers visibility, SetterDelegate setterDelegate) {
            SetterDelegate = setterDelegate ?? throw new ArgumentNullException(nameof(setterDelegate));
            Access = access;
            Visibility = visibility;
        }

        public void SetValue(object value) {
            SetValue(null, value);
        }

        public void SetValue(object instance, object value) {
            if((instance == null) && ((Access & AccessModifiers.Static) == 0))
                throw new ArgumentNullException(nameof(instance), "Can not set value without an instance");

            SetterDelegate.Invoke(instance, value);
        }
    }
}
