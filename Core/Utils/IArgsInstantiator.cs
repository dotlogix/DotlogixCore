namespace DotLogix.Core.Utils {
    /// <summary>
    /// An interface to instantiate types with arguments
    /// </summary>
    public interface IArgsInstantiator {
        /// <summary>
        /// Get or create a new instance
        /// </summary>
        object GetInstance(params object[] args);
    }
}