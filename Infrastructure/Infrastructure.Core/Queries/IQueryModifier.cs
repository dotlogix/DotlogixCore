using System.Linq;

namespace DotLogix.Infrastructure.Queries {
    public interface IQueryModifier<TEntity> : IQueryModifier<TEntity, TEntity> {
        
    }
    
    public interface IQueryModifier<TSource, TTarget> {
        IQueryable<TTarget> Apply(IEntityContext entityContext, IQueryable<TSource> query);
    }
}
