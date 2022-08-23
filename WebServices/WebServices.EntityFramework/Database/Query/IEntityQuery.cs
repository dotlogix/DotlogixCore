using System.Linq;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Database; 

public interface IEntityQuery<TEntity> : IEntityQuery<TEntity, TEntity> {
        
}
    
public interface IEntityQuery<TSource, TTarget> {
    IQueryable<TTarget> Apply(IEntityContext entityContext, IQueryable<TSource> query);
}