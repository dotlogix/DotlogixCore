using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Mappers {
    /// <summary>
    /// A static class class helping to create type to type mappers
    /// </summary>
    public static class Mappers {
        /// <summary>
        /// Creates a mapper based on a configuration function
        /// </summary>
        public static IMapper<TSource, TTarget> Map<TSource, TTarget>(Action<MapBuilder<TSource, TTarget>> configure = null) {
            var mapBuilder = new MapBuilder<TSource, TTarget>();
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }

        /// <summary>
        /// Creates a mapper based on a configuration function auto-maps all supported properties (s.[name] => t.[name])
        /// </summary>
        public static IMapper<TSource, TTarget> AutoMap<TSource, TTarget>(Action<MapBuilder<TSource, TTarget>> configure = null) {
            var mapBuilder = new MapBuilder<TSource, TTarget>();
            mapBuilder.AutoMap();
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }


        /// <summary>
        /// Creates a mapper based on a configuration function
        /// </summary>
        public static IMapper<object, object> Map(Type sourceType, Type targetType, Action <MapBuilder<object, object>> configure = null) {
            var mapBuilder = new MapBuilder<object, object>(sourceType, targetType);
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }
        /// <summary>
        /// Creates a mapper based on a configuration function auto-maps all supported properties (s.[name] => t.[name])
        /// </summary>
        public static IMapper<object, object> AutoMap(Type sourceType, Type targetType, Action<MapBuilder<object, object>> configure = null)
        {
            var mapBuilder = new MapBuilder<object, object>(sourceType, targetType);
            mapBuilder.AutoMap();
            configure?.Invoke(mapBuilder);
            return mapBuilder.Build();
        }
    }

    public class MapBuilder<TSource, TTarget> {
        public DynamicType DynamicSourceType { get; }
        public DynamicType DynamicTargetType { get; }
        public Type SourceType { get; }
        public Type TargetType { get; }

        private readonly List<IMapper<TSource, TTarget>> _mappers = new List<IMapper<TSource, TTarget>>();
        private readonly Dictionary<(string, string), IMapper<TSource, TTarget>> _propertyMappers = new Dictionary<(string, string), IMapper<TSource, TTarget>>();

        public MapBuilder(Type sourceType = null, Type targetType = null) {
            SourceType = sourceType ?? typeof(TSource);
            TargetType = sourceType ?? typeof(TTarget);

            DynamicSourceType = SourceType.CreateDynamicType();
            DynamicTargetType = TargetType.CreateDynamicType();
        }

        public MapBuilder<TSource, TTarget> Map(string sourceProperty, string targetProperty, Action<ValueMapBuilder<TSource, object, TTarget, object>> configure = null) {
            return Map<object, object>(sourceProperty, targetProperty, configure);
        }

        public MapBuilder<TSource, TTarget> AutoMap(params string[] whiteList) {
            return AutoMap(whiteList?.AsEnumerable());
        }

        public MapBuilder<TSource, TTarget> AutoMap(IEnumerable<string> whiteList = null) {
            if(whiteList == null)
                whiteList = DynamicSourceType.Properties.Select(p => p.Name);
            foreach(var property in whiteList)
                Map(property);
            return this;
        }

        public MapBuilder<TSource, TTarget> Map<TValue>(string sourceProperty, string targetProperty, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(sourceProperty, targetProperty, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(Expression<Func<TSource, TSourceValue>> sourceProperty, Expression<Func<TTarget, TTargetValue>> targetProperty, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var sourcePropertyName = sourceProperty.GetTargetProperty()
                                                   ?.Name;
            var targetPropertyName = targetProperty.GetTargetProperty()
                                                   ?.Name;
            return Map(sourcePropertyName, targetPropertyName, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(string sourceProperty, string targetProperty, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            if(sourceProperty == null)
                throw new ArgumentNullException(nameof(sourceProperty));
            if(targetProperty == null)
                throw new ArgumentNullException(nameof(targetProperty));

            var sourceAccessor = DynamicSourceType.GetProperty(sourceProperty);
            var targetAccessor = DynamicTargetType.GetProperty(targetProperty);

            if(sourceAccessor == null || targetAccessor == null)
                return this;

            var valueGetter = new DynamicValueGetter<TSource, TSourceValue>(sourceAccessor);
            var valueSetter = new DynamicValueSetter<TTarget, TTargetValue>(targetAccessor);
            return Map(sourceProperty, targetProperty, valueGetter, valueSetter, configure);
        }

        public MapBuilder<TSource, TTarget> Map(string property, Action<ValueMapBuilder<TSource, object, TTarget, object>> configure = null) {
            return Map<object, object>(property, property, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TValue>(string property, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(property, property, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(string property, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            return Map(property, property, configure);
        }


        public MapBuilder<TSource, TTarget> Map<TValue>(Func<TSource, TValue> getterFunc, Action<TTarget, TValue> setAction, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            var valueGetter = new ValueGetter<TSource, TValue>(getterFunc);
            var valueSetter = new ValueSetter<TTarget, TValue>(setAction);
            return Map<TValue, TValue>(valueGetter, valueSetter, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(Func<TSource, TSourceValue> getterFunc, Action<TTarget, TTargetValue> setAction, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var valueGetter = new ValueGetter<TSource, TSourceValue>(getterFunc);
            var valueSetter = new ValueSetter<TTarget, TTargetValue>(setAction);
            return Map(valueGetter, valueSetter, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TValue>(IValueGetter<TSource, TValue> valueGetter, IValueSetter<TTarget, TValue> valueSetter, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(valueGetter, valueSetter, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(IValueGetter<TSource, TSourceValue> valueGetter, IValueSetter<TTarget, TTargetValue> valueSetter, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var builder = new ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>(valueGetter, valueSetter);
            configure?.Invoke(builder);
            _mappers.Add(builder.Build());
            return this;
        }

        public MapBuilder<TSource, TTarget> Map<TValue>(string sourceProperty, string targetProperty, IValueGetter<TSource, TValue> valueGetter, IValueSetter<TTarget, TValue> valueSetter, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(sourceProperty, targetProperty, valueGetter, valueSetter, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(string sourceProperty, string targetProperty, IValueGetter<TSource, TSourceValue> valueGetter, IValueSetter<TTarget, TTargetValue> valueSetter, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var builder = new ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>(valueGetter, valueSetter);
            configure?.Invoke(builder);
            _propertyMappers[(sourceProperty, targetProperty)] = builder.Build();
            return this;
        }


        public IMapper<TSource, TTarget> Build() {
            return new Mapper<TSource, TTarget>(_mappers.Concat(_propertyMappers.Values));
        }
    }
}
