using System.Collections.Generic;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.EntityFramework.Query;
using DotLogix.Architecture.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// An implementation of the <see cref="IEntitySet{TEntity}"/> interface for entity framework
    /// </summary>
    public class EfEntitySet<TEntity> : EntitySetBase<TEntity> where TEntity : class, ISimpleEntity {
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Create a new instance of <see cref="EfEntitySet{TEntity}"/>
        /// </summary>
        public EfEntitySet(DbSet<TEntity> dbSet) {
            _dbSet = dbSet;
        }

        /// <inheritdoc />
        public override void Add(TEntity entity) {
            _dbSet.Add(entity);
        }

        /// <inheritdoc />
        public override void AddRange(IEnumerable<TEntity> entities) {
            _dbSet.AddRange(entities);
        }

        /// <inheritdoc />
        public override void Remove(TEntity entity) {
            _dbSet.Remove(entity);
        }

        /// <inheritdoc />
        public override void RemoveRange(IEnumerable<TEntity> entities) {
            _dbSet.RemoveRange(entities);
        }

        /// <inheritdoc />
        public override void ReAttach(TEntity entity) {
            var  entry = _dbSet.Attach(entity);
        }

        /// <inheritdoc />
        public override void ReAttachRange(IEnumerable<TEntity> entities) {
            _dbSet.AttachRange(entities);
        }

        /// <inheritdoc />
        public override IQuery<TEntity> Query() {
            return EfQueryableQueryFactory.Instance.CreateQuery(_dbSet);
        }
    }
}