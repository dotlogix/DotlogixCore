using System.Collections.Generic;
using DotLogix.Core.Interfaces;

namespace DotLogix.Core.Utils {
    public interface IReadOnlySettings : ICloneable<IReadOnlySettings>, IEnumerable<KeyValuePair<string, object>> {
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
    }
}