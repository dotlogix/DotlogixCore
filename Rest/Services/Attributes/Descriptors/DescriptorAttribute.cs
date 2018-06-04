using System;
using DotLogix.Core.Rest.Services.Descriptors;

namespace DotLogix.Core.Rest.Services.Attributes.Descriptors {
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class DescriptorAttribute : Attribute {
        public abstract IWebRequestProcessorDescriptor CreateDescriptor();
    }
}