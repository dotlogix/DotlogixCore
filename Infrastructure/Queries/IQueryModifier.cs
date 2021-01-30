using DotLogix.Architecture.Infrastructure.EntityContext;

namespace DotLogix.Architecture.Infrastructure.Queries {
    public interface IQueryModifier<T> {
        IQuery<T> Apply(IEntityContext entityContext, IQuery<T> query);
    }
}
