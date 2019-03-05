﻿using System;
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Caching;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils {
    public class Replicator {
        private readonly Cache<Type, IReadOnlyList<DynamicProperty>> _replicatorCache = new Cache<Type, IReadOnlyList<DynamicProperty>>(TimeSpan.FromMinutes(15));
        public static Replicator Instance { get; } = new Replicator();

        public T FlatClone<T>(T value) where T:new() {
            return (T)FlatClone(typeof(T), value, new T());
        }

        public object FlatClone(object value) {
            var type = value.GetType();
            return FlatClone(type, value, type.Instantiate());
        }

        private object FlatClone(Type type, object source, object target)
        {
            if(_replicatorCache.TryRetrieve(type, out var properties) == false)
                properties = CreateMapping(type);
            _replicatorCache.Store(type, properties, TimeSpan.FromMinutes(15));

            foreach (var dynamicProperty in properties)
                dynamicProperty.SetValue(target, dynamicProperty.GetValue(source));
            return target;
        }

        private IReadOnlyList<DynamicProperty> CreateMapping(Type type) {
            return type.CreateDynamicType(MemberTypes.Property).Properties;
        }
    }
}