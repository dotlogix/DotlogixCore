// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PreProcessorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class PreProcessorAttribute : Attribute {
        public abstract IWebRequestProcessor CreateProcessor();
    }
}
