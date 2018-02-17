// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PostProcessorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class PostProcessorAttribute : Attribute {
        public abstract IWebRequestProcessor CreateProcessor();
    }
}
