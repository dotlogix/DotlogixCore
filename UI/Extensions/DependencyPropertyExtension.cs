// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DependencyPropertyExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.ComponentModel;
using System.Windows;
#endregion

namespace DotLogix.UI.Extensions {
    public static class DependencyPropertyExtension {
        public static void AddChangedHandler(this DependencyProperty property, object owner, EventHandler handler) {
            var descriptor = DependencyPropertyDescriptor.FromProperty(property, property.OwnerType);
            descriptor.AddValueChanged(owner, handler);
        }

        public static void RemoveChangedHandler(this DependencyProperty property, object owner, EventHandler handler) {
            var descriptor = DependencyPropertyDescriptor.FromProperty(property, property.OwnerType);
            descriptor.RemoveValueChanged(owner, handler);
        }
    }
}
