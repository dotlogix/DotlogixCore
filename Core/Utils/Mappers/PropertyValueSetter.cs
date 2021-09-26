using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Mappers {
    /// <inheritdoc />
    public class PropertyValueSetter<TTarget, TValue> : ValueSetterBase<TTarget, TValue> {
        public DynamicAccessor Accessor { get; }

        /// <inheritdoc />
        public PropertyValueSetter(DynamicAccessor accessor) : base(accessor.ReflectedType, accessor.ValueType) {
            Accessor = accessor;
        }

        protected override bool TrySetValue(TTarget instance, TValue value) {
            if(instance == null)
                return false;
            
            Accessor.SetValue(instance, value);
            return true;
        }
    }
}