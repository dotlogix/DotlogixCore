using System;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Rest.Services.Descriptors {
    public class RouteDescriptor : IRouteDescriptor {
        public string Name { get; }
        public IReadOnlySettings Settings { get; }

        public RouteDescriptor(string name, IReadOnlySettings settings) {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
    }
}