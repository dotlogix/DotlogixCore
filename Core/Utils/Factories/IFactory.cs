namespace DotLogix.Core.Utils.Factories {
    /// <summary>
    /// An interface to instantiate types
    /// </summary>
    public interface IFactory {
        /// <summary>
        /// Get or create a new instance
        /// </summary>
        object GetInstance();
    }
    
    /// <summary>
    /// An interface to instantiate types
    /// </summary>
    public interface IFactory<T> : IFactory {
        /// <summary>
        /// Get or create a new instance
        /// </summary>
        new T GetInstance();
    }
}