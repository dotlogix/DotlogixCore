namespace DotLogix.Core.Utils.Factories; 

/// <summary>
/// An interface to instantiate types with arguments
/// </summary>
public interface IArgsFactory {
    /// <summary>
    /// Get or create a new instance
    /// </summary>
    object GetInstance(params object[] args);
}
    
/// <summary>
/// An interface to instantiate types with arguments
/// </summary>
public interface IArgsFactory<T> : IArgsFactory{
    /// <summary>
    /// Get or create a new instance
    /// </summary>
    new T GetInstance(params object[] args);
}