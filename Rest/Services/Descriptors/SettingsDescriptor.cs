using DotLogix.Core.Utils;

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class SettingsDescriptor : RouteDescriptorBase {
        public IReadOnlySettings Settings { get; }
        public SettingsDescriptor(IReadOnlySettings settings) {
            Settings = settings;
        }
    }
}