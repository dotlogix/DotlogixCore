using DotLogix.Core.Extensions;

namespace DotLogix.Core.Utils.Mappers {
    public class ValueAutoConvertingMapper<TSource, TSourceValue, TTarget, TTargetValue> : ValueMapperBase<TSource, TSourceValue, TTarget, TTargetValue> {
        public IValueGetter<TSource, TSourceValue> Getter { get; }
        public IValueSetter<TTarget, TTargetValue> Setter { get; }

        
        /// <summary>
        /// Creates a new instance of <see cref="ValueAutoConvertingMapper"/>
        /// </summary>
        public ValueAutoConvertingMapper(IValueGetter<TSource, TSourceValue> getter, IValueSetter<TTarget, TTargetValue> setter) {
            Getter = getter;
            Setter = setter;
        }

        /// <inheritdoc />
        public override void Map(TSource source, TTarget target) {
            if(Getter.TryGet(source, out var value) == false)
                return;

            if(value.TryConvertTo(out TTargetValue targetValue) == false)
                return;

            Setter.TrySet(target, targetValue);
        }
    }
}