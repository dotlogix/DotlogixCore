// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DescriptorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Services.Descriptors;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Descriptors {
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class DescriptorAttribute : Attribute {
        public abstract IRouteDescriptor CreateDescriptor();
    }
}
