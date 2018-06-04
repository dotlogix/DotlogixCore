using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Services.Descriptors;

namespace DotLogix.Core.Rest.Services.Attributes.Descriptors {
    public class DynamicDescriptorAttributeAttribute : DescriptorAttribute {
        public Type RequestResultWriterType { get; }
        public DynamicDescriptorAttributeAttribute(Type requestResultWriterType) {
            if(requestResultWriterType.IsAssignableTo(typeof(IWebRequestProcessorDescriptor)) == false)
                throw new ArgumentException($"Type {requestResultWriterType.GetFriendlyName()} is not assignable to {nameof(IWebRequestProcessorDescriptor)}", nameof(requestResultWriterType));

            this.RequestResultWriterType = requestResultWriterType;
        }

        public override IWebRequestProcessorDescriptor CreateDescriptor() {
            return RequestResultWriterType?.Instantiate<IWebRequestProcessorDescriptor>();
        }
    }
}