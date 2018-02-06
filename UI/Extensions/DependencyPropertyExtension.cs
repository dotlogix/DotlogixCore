// ==================================================
// Copyright 2016(C) , DotLogix
// File:  DependencyPropertyExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
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
