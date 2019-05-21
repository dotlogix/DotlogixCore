using System.Collections.Generic;
using DotLogix.Architecture.Common.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.EntityFramework.Query;
using DotLogix.Architecture.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public class EfEntitySet<TEntity> : EntitySetBase<TEntity> where TEntity : class, ISimpleEntity {
        private readonly DbSet<TEntity> _dbSet;

        public EfEntitySet(DbSet<TEntity> dbSet) {
            _dbSet = dbSet;
        }

        public override void Add(TEntity entity) {
            _dbSet.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities) {
            _dbSet.AddRange(entities);
        }

        public override void Remove(TEntity entity) {
            _dbSet.Remove(entity);
        }

        public override void RemoveRange(IEnumerable<TEntity> entities) {
            _dbSet.RemoveRange(entities);
        }

        public override void ReAttach(TEntity entity) {
            var  entry = _dbSet.Attach(entity);
        }

        public override void ReAttachRange(IEnumerable<TEntity> entities) {
            _dbSet.AttachRange(entities);
        }

        public override IQuery<TEntity> Query() {
            return EfQueryableQueryFactory.Instance.CreateQuery(_dbSet);
        }
    }
}