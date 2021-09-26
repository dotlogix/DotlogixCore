using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Mappers {
    public class MapBuilder<TSource, TTarget> {
        public DynamicType DynamicSourceType { get; }
        public DynamicType DynamicTargetType { get; }
        public Type SourceType { get; }
        public Type TargetType { get; }

        protected List<IMapper<TSource, TTarget>> CustomMappers { get; } = new List<IMapper<TSource, TTarget>>();
        protected Dictionary<(string, string), IMapper<TSource, TTarget>> PropertyMappers { get; } = new Dictionary<(string, string), IMapper<TSource, TTarget>>();

        public MapBuilder(Type sourceType = null, Type targetType = null) {
            SourceType = sourceType ?? typeof(TSource);
            TargetType = sourceType ?? typeof(TTarget);

            DynamicSourceType = SourceType.CreateDynamicType();
            DynamicTargetType = TargetType.CreateDynamicType();
        }

        public MapBuilder<TSource, TTarget> AutoMap(params string[] whiteList) {
            return AutoMap(whiteList?.AsEnumerable());
        }

        public virtual MapBuilder<TSource, TTarget> AutoMap(IEnumerable<string> whiteList = null) {
            whiteList ??= DynamicSourceType.Properties.Select(p => p.Name);
            foreach(var property in whiteList)
                Map(property);
            return this;
        }

        public MapBuilder<TSource, TTarget> Map(string sourceProperty, string targetProperty, Action<ValueMapBuilder<TSource, object, TTarget, object>> configure = null) {
            return Map<object, object>(sourceProperty, targetProperty, configure);
        }

        public MapBuilder<TSource, TTarget> Map(string property, Action<ValueMapBuilder<TSource, object, TTarget, object>> configure = null) {
            return Map<object, object>(property, property, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TValue>(IValueGetter<TSource, TValue> valueGetter, IValueSetter<TTarget, TValue> valueSetter, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(valueGetter, valueSetter, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TValue>(string property, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(property, property, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TValue>(string sourceProperty, string targetProperty, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(sourceProperty, targetProperty, configure);
        }

        public MapBuilder<TSource, TTarget> Map<TValue>(string sourceProperty, string targetProperty, IValueGetter<TSource, TValue> valueGetter, IValueSetter<TTarget, TValue> valueSetter, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(sourceProperty, targetProperty, valueGetter, valueSetter, configure);
        }
        public MapBuilder<TSource, TTarget> Map<TValue>(Func<TSource, TValue> getterFunc, Action<TTarget, TValue> setAction, Action<ValueMapBuilder<TSource, TValue, TTarget, TValue>> configure = null) {
            return Map<TValue, TValue>(getterFunc, setAction, configure);
        }
        public virtual MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(string property, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            return Map(property, property, configure);
        }

        public virtual MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(string sourceProperty, string targetProperty, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            if(sourceProperty == null)
                throw new ArgumentNullException(nameof(sourceProperty));
            if(targetProperty == null)
                throw new ArgumentNullException(nameof(targetProperty));

            var sourceAccessor = DynamicSourceType.GetProperty(sourceProperty);
            var targetAccessor = DynamicTargetType.GetProperty(targetProperty);

            if(sourceAccessor == null || targetAccessor == null)
                return this;

            var valueGetter = new PropertyValueGetter<TSource, TSourceValue>(sourceAccessor);
            var valueSetter = new PropertyValueSetter<TTarget, TTargetValue>(targetAccessor);
            return Map(sourceProperty, targetProperty, valueGetter, valueSetter, configure);
        }

        protected virtual MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(string sourceProperty, string targetProperty, IValueGetter<TSource, TSourceValue> valueGetter, IValueSetter<TTarget, TTargetValue> valueSetter, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var builder = new ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>(valueGetter, valueSetter);
            configure?.Invoke(builder);
            PropertyMappers[(sourceProperty, targetProperty)] = builder.Build();
            return this;
        }

        protected virtual MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(IValueGetter<TSource, TSourceValue> valueGetter, IValueSetter<TTarget, TTargetValue> valueSetter, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var builder = new ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>(valueGetter, valueSetter);
            configure?.Invoke(builder);
            CustomMappers.Add(builder.Build());
            return this;
        }

        public virtual MapBuilder<TSource, TTarget> Map<TSourceValue, TTargetValue>(Func<TSource, TSourceValue> getterFunc, Action<TTarget, TTargetValue> setAction, Action<ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue>> configure = null) {
            var valueGetter = new ValueGetter<TSource, TSourceValue>(getterFunc);
            var valueSetter = new ValueSetter<TTarget, TTargetValue>(setAction);
            return Map(valueGetter, valueSetter, configure);
        }


        public virtual IMapper<TSource, TTarget> Build() {
            return new Mapper<TSource, TTarget>(CustomMappers.Concat(PropertyMappers.Values));
        }
    }
}
