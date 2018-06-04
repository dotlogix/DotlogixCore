namespace DotLogix.Core.Rest.Services.Descriptors {
    public abstract class WebRequestProcessorDescriptorBase : IWebRequestProcessorDescriptor {
        protected WebRequestProcessorDescriptorBase(string name) {
            Name = name;
        }

        protected WebRequestProcessorDescriptorBase()
        {
            Name = GetType().Name;
        }

        public string Name { get; }
    }
}