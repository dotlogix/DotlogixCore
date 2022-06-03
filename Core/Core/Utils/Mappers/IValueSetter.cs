using System;

namespace DotLogix.Core.Utils.Mappers {
    /// <summary>
    /// An interface representation value setters
    /// </summary>
    public interface IValueSetter<T, TValue> { 
        /// <summary>
        /// The source type
        /// </summary>
        Type InstanceType { get; }

        /// <summary>
        /// The value type
        /// </summary>
        Type ValueType { get; }
        
        /// <summary>
        /// Add a pre-conditions executed before a value will be set
        /// </summary>
        void AddPreCondition(Func<T, bool> conditionFunc);
        /// <summary>
        /// Add a pre-conditions executed before a value will be set
        /// </summary>
        void AddPreCondition(Func<T, TValue, bool> conditionFunc);
        /// <summary>
        /// Tries to set a value
        /// </summary>
        bool TrySet(T instance, TValue value);
    }
}