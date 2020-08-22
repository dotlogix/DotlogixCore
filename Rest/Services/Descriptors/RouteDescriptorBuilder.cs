using DotLogix.Core.Utils;

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class RouteDescriptorBuilder {
        public string Name { get; set; }
        public ISettings Settings { get; set; } = new Settings();

        public RouteDescriptorBuilder UseName(string name) {
            Name = name;
            return this;
        }

        public RouteDescriptorBuilder UseValue(string key, object value) {
            Settings.Set(key, value);
            return this;
        }

        public RouteDescriptor Build() {
            return new RouteDescriptor(Name, Settings);
        }
    }
}