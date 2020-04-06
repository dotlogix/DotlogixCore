using DotLogix.Core.Rest.Services.Descriptors;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Rest.Services {
    public class SettingsDescriptor : RouteDescriptorBase {
        public IReadOnlySettings Settings { get; }
        public SettingsDescriptor(IReadOnlySettings settings) {
            Settings = settings;
        }
    }
}