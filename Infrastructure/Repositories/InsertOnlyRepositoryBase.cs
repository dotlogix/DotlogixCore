using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;

namespace DotLogix.Architecture.Infrastructure.Repositories {
    public class InsertOnlyRepositoryBase<TEntity> : RepositoryBase<TEntity>, IInsertOnlyRepository<TEntity> where TEntity : class, IInsertOnlyEntity {
        public InsertOnlyRepositoryBase(IEntitySetProvider entitySetProvider, bool allowCaching = true) : base(entitySetProvider, allowCaching) { }

        protected override IEntitySet<TEntity> OnModifyEntitySet(IEntitySet<TEntity> set) {
            set = new InsertOnlyEntitySetDecorator<TEntity>(set);
            if (AllowCaching)
                set = new GuidIndexedEntitySetDecorator<TEntity>(set, OnCreateCache());
            return set;
        }
    }
}