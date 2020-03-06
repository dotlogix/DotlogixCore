using System;
using System.Collections.Concurrent;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Delegates;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Reflection.Projections {
    /// <summary>
    /// A projection of two value accessors including an equality check
    /// </summary>
    public class OptionalProjection : Projection {
        private static readonly ConcurrentDictionary<Type, Func<object, (bool, object)>> CachedFuncs = new ConcurrentDictionary<Type, Func<object, (bool, object)>>();

        /// <summary>
        /// Creates a new instance of <see cref="OptionalProjection"/>
        /// </summary>
        public OptionalProjection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter, SetterDelegate rightSetter)
        : base(leftGetter, rightGetter, leftSetter, rightSetter) { }

        /// <inheritdoc />
        public override void ProjectLeftToRight(object left, object right) {
            var leftValue = LeftGetter.Invoke(left);
            if (ShouldProject(ref leftValue))
                RightSetter.Invoke(right, leftValue);
        }

        /// <inheritdoc />
        public override void ProjectRightToLeft(object left, object right) {
            var rightValue = RightGetter.Invoke(right);
            if (ShouldProject(ref rightValue))
                RightSetter.Invoke(right, rightValue);
        }

        private bool ShouldProject(ref object obj) {
            var shouldProjectFunc = CachedFuncs.GetOrAdd(obj.GetType(), CreateDynamicType);
            if(shouldProjectFunc == null)
                return true;

            var (success, result) = shouldProjectFunc.Invoke(obj);
            obj = result;
            return success;
        }

        private Func<object, (bool, object)> CreateDynamicType(Type type) {
            if(type.IsAssignableToOpenGeneric(typeof(Optional<>))) {
                var isDefinedProperty = type.GetProperty("IsDefined").CreateDynamicGetter().GetterDelegate;
                var valueProperty = type.GetProperty("Value").CreateDynamicGetter().GetterDelegate;


                return obj => {
                           if(isDefinedProperty.Invoke(obj) as bool? == true)
                               return (true, valueProperty.Invoke(obj));
                           return (true, null);
                       };
            }
            return null;
        }
    }
}