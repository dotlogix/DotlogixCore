using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class MethodDescriptor : WebRequestProcessorDescriptorBase {
        public DynamicInvoke DynamicInvoke { get; }
        public MethodDescriptor(DynamicInvoke dynamicInvoke) {
            DynamicInvoke = dynamicInvoke;
        }
    }
}