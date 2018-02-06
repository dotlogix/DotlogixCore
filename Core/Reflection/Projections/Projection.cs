using DotLogix.Core.Reflection.Delegates;

namespace DotLogix.Core.Reflection.Projections {
    public class Projection : IProjection
    {
        public GetterDelegate LeftGetter { get; }
        public GetterDelegate RightGetter { get; }
        public SetterDelegate LeftSetter { get; }
        public SetterDelegate RightSetter { get; }

        public Projection(GetterDelegate leftGetter, GetterDelegate rightGetter, SetterDelegate leftSetter, SetterDelegate rightSetter)
        {
            LeftGetter = leftGetter;
            RightGetter = rightGetter;
            LeftSetter = leftSetter;
            RightSetter = rightSetter;
        }

        public virtual void ProjectLeftToRight(object left, object right)
        {
            RightSetter.Invoke(right, LeftGetter.Invoke(left));
        }
        public virtual void ProjectRightToLeft(object left, object right)
        {
            LeftSetter.Invoke(left, RightGetter.Invoke(left));
        }
    }
}