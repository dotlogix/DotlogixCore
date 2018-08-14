// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AuthenticationDescriptorAttribute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Services.Attributes.Descriptors;
using DotLogix.Core.Rest.Services.Descriptors;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication {
    public class AuthenticationDescriptorAttribute : DescriptorAttribute {
        public bool RequiresAuthentication { get; }

        public AuthenticationDescriptorAttribute(bool requiresAuthentication) {
            RequiresAuthentication = requiresAuthentication;
        }

        public override IWebRequestProcessorDescriptor CreateDescriptor() {
            return new AuthenticationDescriptor(RequiresAuthentication);
        }
    }
}
