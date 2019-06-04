using System.Text.RegularExpressions;

namespace DotLogix.Core.Extensions
{
    /// <summary>
    /// A static class providing extension methods for <see cref="Regex"/>
    /// </summary>
    public static class RegexExtensions {
        /// <summary>
        /// Get the value from a <see cref="Group"/> or a default value if the group does not match
        /// </summary>
        public static string GetValueOrDefault(this Group group, string defaultValue = default) {
            return group.Success ? group.Value : defaultValue;
        }
    }
}
