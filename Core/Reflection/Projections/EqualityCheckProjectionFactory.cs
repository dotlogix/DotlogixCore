using DotLogix.Core.Reflection.Delegates;

namespace DotLogix.Core.Reflection.Projections {
    public class EqualityCheckProjectionFactory : ProjectionFactory {
        protected override IProjection CreateProjection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter,
                                                        SetterDelegate rightSetter) {
            return new EqualityCheckProjection(leftGetter, rightGetter, leftSetter, rightSetter);
        }
    }
}