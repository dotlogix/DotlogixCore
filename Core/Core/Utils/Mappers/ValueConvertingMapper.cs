using System;

namespace DotLogix.Core.Utils.Mappers; 

public class ValueConvertingMapper<TSource, TSourceValue, TTarget, TTargetValue> : ValueMapperBase<TSource, TSourceValue, TTarget, TTargetValue> {
    public IValueGetter<TSource, TSourceValue> Getter { get; }
    public IValueSetter<TTarget, TTargetValue> Setter { get; }
    public Func<TSourceValue, TTargetValue> Converter { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="ValueConvertingMapper"/>
    /// </summary>
    public ValueConvertingMapper(IValueGetter<TSource, TSourceValue> getter, IValueSetter<TTarget, TTargetValue> setter, Func<TSourceValue, TTargetValue> converter) {
        Getter = getter;
        Setter = setter;
        Converter = converter;
    }

    /// <inheritdoc />
    public override void Map(TSource source, TTarget target) {
        if(Getter.TryGet(source, out var value) == false)
            return;
        Setter.TrySet(target, Converter.Invoke(value));
    }
}