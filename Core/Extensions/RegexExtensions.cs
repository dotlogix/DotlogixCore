using System.Text.RegularExpressions;

namespace DotLogix.Core.Extensions
{
    public static class RegexExtensions {
        public static string GetValueOrDefault(this Group group, string defaultValue = default) {
            return group.Success ? group.Value : defaultValue;
        }
    }
}
