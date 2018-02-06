namespace DotLogix.Core.Reflection.Projections
{
    public interface IProjection
    {
        void ProjectLeftToRight(object left, object right);
        void ProjectRightToLeft(object left, object right);
    }
}