#region using
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DotLogix.Core.Utils.Mappers {
    /// <summary>
    ///     An implementation of the <see cref="IValueGetter{TSource,TValue}" interface />
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class ValueGetterBase<TSource, TValue> : IValueGetter<TSource, TValue> {
        protected ValueGetterBase() {
            InstanceType = typeof(TSource);
            ValueType = typeof(TValue); 
        }
        
        protected ValueGetterBase(Type sourceType, Type valueType) {
            InstanceType = sourceType ?? typeof(TSource);
            ValueType = valueType ?? typeof(TValue);
        }

        /// <summary>
        ///     A list of pre-conditions executed before a value will be resolved
        /// </summary>
        protected List<Func<TSource, bool>> PreConditions { get; } = new();

        /// <summary>
        ///     A list of post-conditions executed after a value has been resolved
        /// </summary>
        protected List<Func<TSource, TValue, bool>> PostConditions { get; } = new();

        /// <inheritdoc />
        public Type InstanceType { get; }

        /// <inheritdoc />
        public Type ValueType { get; }

        /// <summary>
        ///     Add a pre-conditions executed before a value will be resolved
        /// </summary>
        public void AddPreCondition(Func<TSource, bool> conditionFunc) {
            PreConditions.Add(conditionFunc);
        }

        /// <summary>
        ///     Add a post-conditions executed after a value has been resolved
        /// </summary>
        public void AddPostCondition(Func<TSource, TValue, bool> conditionFunc) {
            PostConditions.Add(conditionFunc);
        }

        /// <summary>
        ///     Tries to resolve a value
        /// </summary>
        public bool TryGet(TSource instance, out TValue value) {
            if(CheckPreConditions(instance) && TryGetValue(instance, out value) && CheckPostConditions(instance, value))
                return true;
            value = default;
            return false;
        }

        /// <summary>
        ///     Tries to resolve a value
        /// </summary>
        protected abstract bool TryGetValue(TSource source, out TValue value);

        /// <summary>
        ///     Check if all pre-conditions are fulfilled
        /// </summary>
        protected bool CheckPreConditions(TSource source) {
            return (PreConditions.Count == 0) || PreConditions.All(c => c.Invoke(source));
        }

        /// <summary>
        ///     Check if all post-conditions are fulfilled
        /// </summary>
        protected bool CheckPostConditions(TSource source, TValue value) {
            return (PostConditions.Count == 0) || PostConditions.All(c => c.Invoke(source, value));
        }
    }
}
