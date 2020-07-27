namespace DotLogix.Core.Utils {
    /// <summary>
    /// An interface to instantiate types
    /// </summary>
    public interface IInstantiator {
        /// <summary>
        /// Get or create a new instance
        /// </summary>
        object GetInstance();
    }
    
    /// <summary>
    /// An interface to instantiate types
    /// </summary>
    public interface IInstantiator<T> : IInstantiator {
        /// <summary>
        /// Get or create a new instance
        /// </summary>
        new T GetInstance();
    }
}