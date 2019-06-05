using System;

namespace DotLogix.Core.Utils {
    /// <summary>
    /// An interface to instantiate types
    /// </summary>
    public interface IInstantiator {
        /// <summary>
        /// Get or create an instance
        /// </summary>
        object GetInstance();
    }

    /// <summary>
    /// An interface to instantiate types with arguments
    /// </summary>
    public interface IArgsInstantiator {
        /// <summary>
        /// Get or create an instance
        /// </summary>
        object GetInstance(params object[] args);
    }
}