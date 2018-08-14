// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AuthenticationDescriptor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  02.06.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Core.Rest.Services.Descriptors;
#endregion

namespace DotLogix.Core.Rest.Services.Processors.Authentication {
    public class AuthenticationDescriptor : WebRequestProcessorDescriptorBase {
        public bool RequiresAuthentication { get; }

        public AuthenticationDescriptor(bool requiresAuthentication) {
            RequiresAuthentication = requiresAuthentication;
        }
    }
}
