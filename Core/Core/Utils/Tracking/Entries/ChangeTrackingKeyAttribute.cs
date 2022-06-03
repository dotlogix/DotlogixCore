// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ChangeTrackingKeyAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Utils.Tracking.Entries {
    /// <summary>
    /// Set this property as key for change tracking index 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ChangeTrackingKeyAttribute : Attribute { }
}
