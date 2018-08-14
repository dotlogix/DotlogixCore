// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SupportsFlagsAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Enumeration {
    [AttributeUsage(AttributeTargets.Class)]
    public class SupportsFlagsAttribute : Attribute { }
}
