using DotLogix.Core.Extensions;

namespace DotLogix.Core.Utils.Mappers {
    public class ValueAutoConvertingMapper<TSource, TSourceValue, TTarget, TTargetValue> : IMapper<TSource, TTarget> {
        public IValueGetter<TSource, TSourceValue> Getter { get; }
        public IValueSetter<TTarget, TTargetValue> Setter { get; }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public ValueAutoConvertingMapper(IValueGetter<TSource, TSourceValue> getter, IValueSetter<TTarget, TTargetValue> setter) {
            Getter = getter;
            Setter = setter;
        }

        public void Map(TSource source, TTarget target) {
            if(Getter.TryGet(source, out var value) == false)
                return;

            if(value.TryConvertTo(out TTargetValue targetValue) == false)
                return;

            Setter.TrySet(target, targetValue);
        }
    }
}