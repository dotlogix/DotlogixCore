using System.Collections.Generic;
using DotLogix.Core.Interfaces;

namespace DotLogix.Core.Utils {
    public interface IReadOnlySettings : ICloneable<IReadOnlySettings>, IEnumerable<KeyValuePair<string, object>> {
        /// <summary>
        /// Gets ot sets a parent settings object.
        /// If a setting is not defined the setting will be inherited in the same order as in the collection
        /// </summary>
        IReadOnlySettings Inherits { get; }

        /// <summary>
        /// Gets the self defined settings
        /// </summary>
        IReadOnlySettings OwnSettings { get; }
        /// <summary>
        /// Gets the keys of self defined settings
        /// </summary>
        IEnumerable<string> OwnKeys { get; }
        /// <summary>
        /// Gets the amount of self defined settings
        /// </summary>
        int OwnCount { get; }

        /// <summary>
        /// Gets the distinct keys of self defined and inherited settings
        /// </summary>
        IEnumerable<string> Keys { get; }
        
        /// <summary>
        /// Gets the amount of self defined and inherited settings
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the setting assigned to the key or null if undefined
        /// </summary>
        Optional<object> this[string key] { get; }

        /// <summary>
        /// Gets the setting assigned to the key or a default value if undefined
        /// </summary>
        Optional<object> Get(string key);

        /// <summary>
        /// Gets the setting assigned to the key or a default value if undefined
        /// </summary>
        object Get(string key, object defaultValue);

        /// <summary>
        /// Gets the setting assigned to the key converted to the type specified or a default value if undefined
        /// </summary>
        Optional<T> Get<T>(string key);

        /// <summary>
        /// Gets the setting assigned to the key converted to the type specified or a default value if undefined
        /// </summary>
        T Get<T>(string key, T defaultValue);

        /// <summary>
        /// Tries to get the setting assigned to the key
        /// </summary>
        /// <returns>true if the setting is defined, otherwise false</returns>
        bool TryGet(string key, out object value);

        /// <summary>
        /// Tries to get the setting assigned to the key converted to the type specified
        /// </summary>
        /// <returns>true if the setting is defined and the value is convertible to the target type, otherwise false</returns>
        bool TryGet<T>(string key, out T value);

        /// <summary>
        /// Cascades all own settings and inherited settings to a new settings instance (order: own, inherited[0]...inherited[n])
        /// </summary>
        ISettings Reduce();

        /// <summary>
        /// Creates a new settings object which inherits the current one
        /// </summary>
        ISettings Inherit();
    }

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
