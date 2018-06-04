using System;
using System.Collections.Generic;
using System.Text;
using DotLogix.Core.Rest.Services.Descriptors;

namespace DotLogix.Core.Rest.Services.Processors.Authentication
{
    public class AuthenticationDescriptor : WebRequestProcessorDescriptorBase {
        public AuthenticationDescriptor(bool requiresAuthentication) {
            RequiresAuthentication = requiresAuthentication;
        }

        public bool RequiresAuthentication { get; }
    }
}
