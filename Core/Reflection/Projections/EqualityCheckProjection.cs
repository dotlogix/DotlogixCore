using DotLogix.Core.Reflection.Delegates;

namespace DotLogix.Core.Reflection.Projections {
    public class EqualityCheckProjection : Projection {
        public EqualityCheckProjection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter, SetterDelegate rightSetter)
            : base(leftGetter, rightGetter, leftSetter, rightSetter) { }

        public override void ProjectLeftToRight(object left, object right) {
            if(ShouldProject(left, right, out var newValue, out _))
                RightSetter.Invoke(right, newValue);
        }

        public override void ProjectRightToLeft(object left, object right) {
            if (ShouldProject(left, right, out _, out var rightValue))
                LeftSetter.Invoke(left, rightValue);
        }

        private bool ShouldProject(object left, object right, out object leftValue, out object rightValue) {
            leftValue = LeftGetter.Invoke(left);
            rightValue = RightGetter.Invoke(right);

            return Equals(leftValue, rightValue) == false;
        }
    }
}