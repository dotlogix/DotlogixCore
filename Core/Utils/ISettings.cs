using System.Collections.Generic;

namespace DotLogix.Core.Utils {
    /// <summary>
    /// An interface to represent dynamic settings
    /// </summary>
    public interface ISettings : IReadOnlySettings {
        /// <summary>
        /// Adds or updates a setting with the corresponding key
        /// </summary>
        void Set(string key, object value = default);

        /// <summary>
        /// Removes the setting to the key
        /// </summary>
        /// <returns>true if key was removed, otherwise false</returns>
        bool Reset(string key);

        /// <summary>
        /// Sets a number of settings based on the values and their keys
        /// </summary>
        void Apply(IEnumerable<KeyValuePair<string, object>> values, bool replaceExisting = true);
    }
}
