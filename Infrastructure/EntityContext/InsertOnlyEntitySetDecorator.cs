using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public class InsertOnlyEntitySetDecorator<TEntity> : EntitySetDecoratorBase<TEntity> where TEntity : class, IInsertOnlyEntity {
        public InsertOnlyEntitySetDecorator(IEntitySet<TEntity> baseEntitySet) : base(baseEntitySet) { }

        public override void Add(TEntity entity) {
            entity.IsActive = true;
            BaseEntitySet.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities) {
            var list = entities.ToList();
            foreach(var entity in list)
                entity.IsActive = true;
            BaseEntitySet.AddRange(list);
        }

        public override void Remove(TEntity entity) {
            entity.IsActive = false;
        }

        public override void RemoveRange(IEnumerable<TEntity> entities) {
            foreach(var entity in entities)
                entity.IsActive = true;
        }

        public override IQuery<TEntity> Query() {
            return BaseEntitySet.Query().Where(e => e.IsActive);
        }
    }
}