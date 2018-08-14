// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MethodDescriptor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class MethodDescriptor : WebRequestProcessorDescriptorBase {
        public DynamicInvoke DynamicInvoke { get; }

        public MethodDescriptor(DynamicInvoke dynamicInvoke) {
            DynamicInvoke = dynamicInvoke;
        }
    }
}
