using System;

namespace DotLogix.Core.Utils.Mappers {
    public class ValueConvertingMapper<TSource, TSourceValue, TTarget, TTargetValue> : IMapper<TSource, TTarget> {
        public IValueGetter<TSource, TSourceValue> Getter { get; }
        public IValueSetter<TTarget, TTargetValue> Setter { get; }
        public Func<TSourceValue, TTargetValue> ConvertValueFunc { get; }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public ValueConvertingMapper(IValueGetter<TSource, TSourceValue> getter, IValueSetter<TTarget, TTargetValue> setter, Func<TSourceValue, TTargetValue> convertValueFunc) {
            Getter = getter;
            Setter = setter;
            ConvertValueFunc = convertValueFunc;
        }

        public void Map(TSource source, TTarget target) {
            if(Getter.TryGet(source, out var value) == false)
                return;
            Setter.TrySet(target, ConvertValueFunc.Invoke(value));
        }
    }
}