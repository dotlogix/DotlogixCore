// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ChangeTrackingKeyAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Tracking.Entries {
    [AttributeUsage(AttributeTargets.Property)]
    public class ChangeTrackingKeyAttribute : Attribute { }
}
