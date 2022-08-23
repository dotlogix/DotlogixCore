// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicSetter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Reflection.Delegates;
#endregion

namespace DotLogix.Core.Reflection.Dynamics {
    /// <summary>
    /// A representation of a setter
    /// </summary>
    public sealed class DynamicSetter {
        /// <summary>
        /// The access modifiers
        /// </summary>
        public AccessModifiers Access { get; }
        /// <summary>
        /// The visibility modifiers
        /// </summary>
        public VisibilityModifiers Visibility { get; }
        /// <summary>
        /// The setter delegate
        /// </summary>
        public SetterDelegate SetterDelegate { get; }

        internal DynamicSetter(AccessModifiers access, VisibilityModifiers visibility, SetterDelegate setterDelegate) {
            SetterDelegate = setterDelegate ?? throw new ArgumentNullException(nameof(setterDelegate));
            Access = access;
            Visibility = visibility;
        }
        /// <summary>
        /// Set the value<br></br>
        /// The field must be static
        /// </summary>
        /// <returns></returns>
        public void SetValue(object value) {
            SetValue(null, value);
        }
        /// <summary>
        /// Set the value
        /// </summary>
        /// <returns></returns>
        public void SetValue(object instance, object value) {
            if((instance == null) && ((Access & AccessModifiers.Static) == 0))
                throw new ArgumentNullException(nameof(instance), "Can not set value without a new instance");

            SetterDelegate.Invoke(instance, value);
        }
    }
}