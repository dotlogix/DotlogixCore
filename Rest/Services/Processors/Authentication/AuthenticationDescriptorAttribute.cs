using System;
using System.Collections.Generic;
using System.Text;
using DotLogix.Core.Rest.Services.Attributes.Descriptors;
using DotLogix.Core.Rest.Services.Attributes.Routes;
using DotLogix.Core.Rest.Services.Descriptors;

namespace DotLogix.Core.Rest.Services.Processors.Authentication
{
    public class AuthenticationDescriptorAttribute : DescriptorAttribute
    {
        public AuthenticationDescriptorAttribute(bool requiresAuthentication) {
            RequiresAuthentication = requiresAuthentication;
        }
        public bool RequiresAuthentication { get; }

        public override IWebRequestProcessorDescriptor CreateDescriptor() {
            return new AuthenticationDescriptor(RequiresAuthentication);
        }
    }
}
