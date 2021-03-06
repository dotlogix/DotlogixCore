﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicDescriptorAttributeAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Services.Descriptors;
#endregion

namespace DotLogix.Core.Rest.Services.Attributes.Descriptors {
    public class DynamicDescriptorAttributeAttribute : DescriptorAttribute {
        public Type RequestResultWriterType { get; }

        public DynamicDescriptorAttributeAttribute(Type requestResultWriterType) {
            if(requestResultWriterType.IsAssignableTo(typeof(IWebRequestProcessorDescriptor)) == false)
                throw new ArgumentException($"Type {requestResultWriterType.GetFriendlyName()} is not assignable to {nameof(IWebRequestProcessorDescriptor)}", nameof(requestResultWriterType));

            RequestResultWriterType = requestResultWriterType;
        }

        public override IWebRequestProcessorDescriptor CreateDescriptor() {
            return RequestResultWriterType?.Instantiate<IWebRequestProcessorDescriptor>();
        }
    }
}
