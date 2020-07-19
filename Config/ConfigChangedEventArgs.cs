using System;

namespace DotLogix.Core.Config {
    /// <summary>
    /// Event args to represent a config file change
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public class ConfigChangedEventArgs<TConfig> : EventArgs{
        /// <summary>
        /// The configuration file
        /// </summary>
        public IConfigurationFile<TConfig> File { get; }
        /// <summary>
        /// The current configuration (equal to <see cref="Previous"/> if <see cref="Success"/> == false)
        /// </summary>
        public TConfig Current { get; }
        /// <summary>
        /// The previous configuration
        /// </summary>
        public TConfig Previous { get; }
        /// <summary>
        /// Check if the current loading attempt succeeded
        /// </summary>
        public bool Success { get; }

        /// <inheritdoc />
        public ConfigChangedEventArgs(IConfigurationFile<TConfig> file, TConfig current, TConfig previous = default, bool success = true) {
            Current = current;
            File = file;
            Previous = previous;
            Success = success;
        }
    }
}