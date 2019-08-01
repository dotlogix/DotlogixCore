using DotLogix.Core.Reflection.Delegates;

namespace DotLogix.Core.Reflection.Projections {
    /// <inheritdoc />
    public class OptionalProjectionFactory : ProjectionFactory
    {
        /// <inheritdoc />
        protected override IProjection CreateProjection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter,
                                                        SetterDelegate rightSetter)
        {
            return new OptionalProjection(leftGetter, rightGetter, leftSetter, rightSetter);
        }
    }
}