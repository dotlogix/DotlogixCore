using System.Collections.Generic;

namespace DotLogix.Core.Utils {
    /// <summary>
    /// A settings collection with multiple layers
    /// </summary>
    public interface ILayeredSettings : IReadOnlySettings {
        /// <summary>
        /// The layers of settings
        /// </summary>
        IEnumerable<IReadOnlySettings> Layers { get; }
        /// <summary>
        /// The topmost layer
        /// </summary>
        IReadOnlySettings CurrentLayer { get; }

        /// <summary>
        /// Add a new layer to the settings
        /// </summary>
        ISettings PushLayer();

        /// <summary>
        /// Removes the topmost layer from the stack
        /// </summary>
        /// <returns></returns>
        IReadOnlySettings PopLayer();
        /// <summary>
        /// Get the topmost layer but don't remove it
        /// </summary>
        /// <returns></returns>
        IReadOnlySettings PeekLayer();
    }
}