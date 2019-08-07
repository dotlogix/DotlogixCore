using System;
using System.ComponentModel;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Utils.Mappers {
    public class ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> {
        private readonly IValueGetter<TSource, TSourceValue> _valueGetter;
        private readonly IValueSetter<TTarget, TTargetValue> _valueSetter;
        private Func<TSourceValue, TTargetValue> _conversionFunc;

        public ValueMapBuilder(IValueGetter<TSource, TSourceValue> valueGetter, IValueSetter<TTarget, TTargetValue> valueSetter) {
            _valueGetter = valueGetter;
            _valueSetter = valueSetter;
        }

        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> ConvertWith(Func<TSourceValue, TTargetValue> conversionFunc) {
            _conversionFunc = conversionFunc;
            return this;
        }
        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> ConvertWith(TypeConverter converter) {
            if(converter.CanConvertFrom(typeof(TSourceValue)))
                _conversionFunc = v => (TTargetValue)converter.ConvertFrom(v);
            else if(converter.CanConvertTo(typeof(TTargetValue)))
                _conversionFunc = v => (TTargetValue)converter.ConvertTo(v, typeof(TTargetValue));
            else
                throw new ArgumentException($"The type converter is not able to convert {typeof(TSourceValue).GetFriendlyName()} to target type {typeof(TTargetValue).GetFriendlyName()}");
            return this;
        }

        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> ConvertWith<TConverter>() where TConverter : TypeConverter, new() {
            return ConvertWith(new TConverter());
        }

        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> GetOnlyIf(Func<TSource, bool> conditionFunc) {
            _valueGetter.AddPreCondition(conditionFunc);
            return this;
        }

        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> GetOnlyIf(Func<TSource, TSourceValue, bool> conditionFunc) {
            _valueGetter.AddPostCondition(conditionFunc);
            return this;
        }
        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> GetOnlyIf(Func<TSourceValue, bool> conditionFunc) {
            _valueGetter.AddPostCondition((s, v) => conditionFunc.Invoke(v));
            return this;
        }

        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> SetOnlyIf(Func<TTarget, bool> conditionFunc) {
            _valueSetter.AddPreCondition(conditionFunc);
            return this;
        }

        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> SetOnlyIf(Func<TTarget, TTargetValue, bool> conditionFunc) {
            _valueSetter.AddPreCondition(conditionFunc);
            return this;
        }
        public ValueMapBuilder<TSource, TSourceValue, TTarget, TTargetValue> SetOnlyIf(Func<TTargetValue, bool> conditionFunc) {
            _valueSetter.AddPreCondition((t, v)=>conditionFunc.Invoke(v));
            return this;
        }

        public IMapper<TSource, TTarget> Build() {
            if(_conversionFunc != null)
                return new ValueConvertingMapper<TSource, TSourceValue, TTarget, TTargetValue>(_valueGetter, _valueSetter, _conversionFunc);
            return new ValueAutoConvertingMapper<TSource, TSourceValue, TTarget, TTargetValue>(_valueGetter, _valueSetter);
        }
    }
}