using System;
using System.Reflection;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Architecture.Infrastructure.Decorators {
    public class IndexedAttribute : EntitySetModifierAttribute {
        public IndexedAttribute(int priority = int.MinValue) : base(priority) { }
        public override Func<IEntitySet<TEntity>, IEntitySet<TEntity>> GetModifierFunc<TEntity>() {
            var entitySetType = typeof(IEntitySet<TEntity>);
            var createEntityCacheType = typeof(Func<EntityIndex<TEntity>>);
            var ctor = typeof(IndexedEntitySetDecorator<>)
                       .MakeGenericType(typeof(TEntity))
                       .CreateDynamicCtor(new Type[] {entitySetType, createEntityCacheType});


            return (entitySet) => (IEntitySet<TEntity>)ctor.Invoke(entitySet, (Func<EntityIndex<TEntity>>)OnCreateCache<TEntity>);
        }

        protected virtual EntityIndex<TEntity> OnCreateCache<TEntity>() where TEntity : ISimpleEntity {
            return new EntityIndex<TEntity>();
        }
    }
}