﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IgnoreChangesAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Tracking.Entries {
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreChangesAttribute : Attribute { }
}
