using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Mappers {
    /// <inheritdoc />
    public class DynamicValueGetter<TSource, TValue> : ValueGetter<TSource, TValue> {
        /// <inheritdoc />
        public DynamicValueGetter(DynamicAccessor accessor) : base(accessor.ReflectedType, accessor.ValueType, s => (TValue)accessor.GetValue(s)) { }
    }
}