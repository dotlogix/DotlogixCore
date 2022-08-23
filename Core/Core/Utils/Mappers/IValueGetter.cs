using System;

namespace DotLogix.Core.Utils.Mappers; 

/// <summary>
/// An interface representation value resolvers
/// </summary>
public interface IValueGetter<T, TValue> {
    /// <summary>
    /// The source type
    /// </summary>
    Type InstanceType { get; }
        
    /// <summary>
    /// The value type
    /// </summary>
    Type ValueType { get; }
        
    /// <summary>
    /// Add a pre-conditions executed before a value will be resolved
    /// </summary>
    void AddPreCondition(Func<T, bool> conditionFunc);
    /// <summary>
    /// Add a post-conditions executed after a value has been resolved
    /// </summary>
    void AddPostCondition(Func<T, TValue, bool> conditionFunc);
    /// <summary>
    /// Tries to resolve a value
    /// </summary>
    bool TryGet(T instance, out TValue value);
}