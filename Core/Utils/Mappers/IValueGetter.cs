using System;

namespace DotLogix.Core.Utils.Mappers {
    /// <summary>
    /// An interface representation value resolvers
    /// </summary>
    public interface IValueGetter<TSource, TValue> {
        /// <summary>
        /// The source type
        /// </summary>
        Type SourceType { get; }
        
        /// <summary>
        /// The value type
        /// </summary>
        Type ValueType { get; }
        
        /// <summary>
        /// Add a pre-conditions executed before a value will be resolved
        /// </summary>
        void AddPreCondition(Func<TSource, bool> conditionFunc);
        /// <summary>
        /// Add a post-conditions executed after a value has been resolved
        /// </summary>
        void AddPostCondition(Func<TSource, TValue, bool> conditionFunc);
        /// <summary>
        /// Tries to resolve a value
        /// </summary>
        bool TryGet(TSource source, out TValue value);
    }
}