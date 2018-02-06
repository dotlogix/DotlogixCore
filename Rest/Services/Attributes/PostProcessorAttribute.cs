// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebRequestPostProcessorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  31.01.2018
// LastEdited:  31.01.2018
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
