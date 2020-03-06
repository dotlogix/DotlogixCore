using System;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Utils.Mappers {
    /// <summary>
    /// An implementation of the <see cref="IValueGetter{TSource,TValue}" interface/>
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class ValueGetterBase<TSource, TValue> : IValueGetter<TSource, TValue> {
        /// <summary>
        /// A list of pre-conditions executed before a value will be resolved
        /// </summary>
        protected List<Func<TSource, bool>> PreConditionFuncs { get; } = new List<Func<TSource, bool>>();

        /// <summary>
        /// A list of post-conditions executed after a value has been resolved
        /// </summary>
        protected List<Func<TSource, TValue, bool>> PostConditionFuncs { get; } = new List<Func<TSource, TValue, bool>>();

        /// <summary>
        /// Add a pre-conditions executed before a value will be resolved
        /// </summary>
        public void AddPreCondition(Func<TSource, bool> conditionFunc) {
            PreConditionFuncs.Add(conditionFunc);
        }
        /// <summary>
        /// Add a post-conditions executed after a value has been resolved
        /// </summary>
        public void AddPostCondition(Func<TSource, TValue, bool> conditionFunc) {
            PostConditionFuncs.Add(conditionFunc);
        }

        /// <summary>
        /// Tries to resolve a value
        /// </summary>
        public bool TryGet(TSource source, out TValue value) {
            if(CheckPreConditions(source) && TryGetValue(source, out value) && CheckPostConditions(source, value))
                return true;
            value = default;
            return false;
        }

        /// <summary>
        /// Tries to resolve a value
        /// </summary>
        protected abstract bool TryGetValue(TSource source, out TValue value);

        /// <summary>
        /// Check if all pre-conditions are fulfilled
        /// </summary>
        protected bool CheckPreConditions(TSource source) {
            return (PreConditionFuncs.Count == 0) || PreConditionFuncs.All(c => c.Invoke(source));
        }

        /// <summary>
        /// Check if all post-conditions are fulfilled
        /// </summary>
        protected bool CheckPostConditions(TSource source, TValue value) {
            return (PostConditionFuncs.Count == 0) || PostConditionFuncs.All(c => c.Invoke(source, value));
        }
    }
}