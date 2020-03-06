using System;
using System.Collections.Generic;
using DotLogix.Core.Caching;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils {
    /// <summary>
    /// A singleton instance of <see cref="Replicator"/>
    /// </summary>
    public class Replicator {
        private readonly Cache<Type, IReadOnlyList<DynamicProperty>> _replicatorCache = new Cache<Type, IReadOnlyList<DynamicProperty>>(TimeSpan.FromMinutes(15));
        /// <summary>
        /// The singleton instance
        /// </summary>
        public static Replicator Instance { get; } = new Replicator();

        /// <summary>
        /// Create a shallow copy of an object
        /// </summary>
        public T FlatClone<T>(T value) where T:new() {
            return  (T)FlatClone(typeof(T), value, new T());
        }

        /// <summary>
        /// Create a shallow copy of an object
        /// </summary>
        public object FlatClone(object value) {
            var type = value.GetType();
            return FlatClone(type, value, type.Instantiate());
        }

        /// <summary>
        /// Create a shallow copy of an object
        /// </summary>
        private object FlatClone(Type type, object source, object target) {
            var properties = _replicatorCache.RetrieveOrCreate(type, CreateMapping, TimeSpan.FromMinutes(15));

            foreach (var dynamicProperty in properties)
                dynamicProperty.SetValue(target, dynamicProperty.GetValue(source));
            return target;
        }

        private IReadOnlyList<DynamicProperty> CreateMapping(Type type) {
            return type.CreateDynamicType().Properties.AsArray();
        }
    }
}