// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MethodDescriptor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class MethodDescriptor : RouteDescriptorBase {
        public DynamicInvoke DynamicInvoke { get; }
        public bool IsAsyncMethod { get; }
        public Type ReturnType { get; }

        public MethodDescriptor(DynamicInvoke dynamicInvoke) {
            DynamicInvoke = dynamicInvoke;
            ReturnType = dynamicInvoke.ReturnType;
            if(ReturnType.IsAssignableToOpenGeneric(typeof(Task<>), out var arguments)) {
                IsAsyncMethod = true;
                ReturnType = arguments[0];
            }
        }
    }
}
