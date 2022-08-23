using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Mappers; 

/// <inheritdoc />
public class PropertyValueGetter<TSource, TValue> : ValueGetterBase<TSource, TValue> {
    public DynamicAccessor Accessor { get; }

    /// <inheritdoc />
    public PropertyValueGetter(DynamicAccessor accessor) : base(accessor.ReflectedType, accessor.ValueType) {
        Accessor = accessor;
    }

    protected override bool TryGetValue(TSource source, out TValue value) {
        if(source == null) {
            value = default;
            return false;
        }
        value = (TValue)Accessor.GetValue(source);
        return true;
    }
}