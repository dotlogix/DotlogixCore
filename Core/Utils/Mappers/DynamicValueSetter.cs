using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Mappers {
    /// <inheritdoc />
    public class DynamicValueSetter<TTarget, TValue> : ValueSetter<TTarget, TValue> {
        /// <inheritdoc />
        public DynamicValueSetter(DynamicAccessor accessor) : base((t, v) => accessor.SetValue(t, v)) { }
    }
}