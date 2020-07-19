using System;

namespace DotLogix.Core.Utils.Mappers {
    /// <summary>
    /// An interface representation value setters
    /// </summary>
    public interface IValueSetter<TTarget, TValue> { 
        /// <summary>
        /// The source type
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// The value type
        /// </summary>
        Type ValueType { get; }
        
        /// <summary>
        /// Add a pre-conditions executed before a value will be set
        /// </summary>
        void AddPreCondition(Func<TTarget, bool> conditionFunc);
        /// <summary>
        /// Add a pre-conditions executed before a value will be set
        /// </summary>
        void AddPreCondition(Func<TTarget, TValue, bool> conditionFunc);
        /// <summary>
        /// Tries to set a value
        /// </summary>
        bool TrySet(TTarget target, TValue value);
    }
}