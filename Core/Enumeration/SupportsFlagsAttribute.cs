// ==================================================
// Copyright 2016(C) , DotLogix
// File:  SupportsFlagsAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Enumeration {
    [AttributeUsage(AttributeTargets.Class)]
    public class SupportsFlagsAttribute : Attribute { }
}
