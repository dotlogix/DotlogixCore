using System;

namespace DotLogix.WebServices.EntityFramework.Expressions {
    public static class VersionExtensions {
        public static Version GetPartial(this Version version, VersionParts parts = VersionParts.Minor) {
            return parts switch {
                VersionParts.Major => new Version(version.Major, 0),
                VersionParts.Minor => new Version(version.Major, version.Minor),
                VersionParts.Build => new Version(version.Major, version.Minor, version.Build),
                VersionParts.Revision => new Version(version.Major, version.Minor, version.Build, version.Revision),
                _ => throw new ArgumentOutOfRangeException(nameof(parts), parts, null)
            };
        }
    }
}