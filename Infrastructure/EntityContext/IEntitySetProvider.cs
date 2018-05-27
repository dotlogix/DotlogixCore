using DotLogix.Architecture.Infrastructure.Entities;

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public interface IEntitySetProvider {
        IEntitySet<TEntity> UseSet<TEntity>() where TEntity : class, ISimpleEntity;
    }
}