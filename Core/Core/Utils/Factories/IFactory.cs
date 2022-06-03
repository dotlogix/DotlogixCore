namespace DotLogix.Core.Utils.Factories {
    /// <summary>
    /// An interface to instantiate types
    /// </summary>
    public interface IFactory {
        /// <summary>
        /// Get or create a new instance
        /// </summary>
        object Create();
    }
    
    /// <summary>
    /// An interface to instantiate types
    /// </summary>
    public interface IFactory<out T> : IFactory {
        /// <summary>
        /// Get or create a new instance
        /// </summary>
        new T Create();
    }
}