using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public abstract class EntitySetDecoratorBase<TEnity> : IEntitySet<TEnity> where TEnity : ISimpleEntity {
        protected IEntitySet<TEnity> BaseEntitySet { get; }

        protected EntitySetDecoratorBase(IEntitySet<TEnity> baseEntitySet) {
            BaseEntitySet = baseEntitySet;
        }

        public virtual IQuery<TEnity> Query() {
            return BaseEntitySet.Query();
        }

        public virtual Task<TEnity> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken)) {
            return BaseEntitySet.GetAsync(id, cancellationToken);
        }

        public virtual Task<IEnumerable<TEnity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default(CancellationToken)) {
            return BaseEntitySet.GetRangeAsync(ids, cancellationToken);
        }

        public virtual Task<TEnity> GetAsync(Guid guid, CancellationToken cancellationToken = default(CancellationToken)) {
            return BaseEntitySet.GetAsync(guid, cancellationToken);
        }

        public virtual Task<IEnumerable<TEnity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default(CancellationToken)) {
            return BaseEntitySet.GetRangeAsync(guids, cancellationToken);
        }

        public virtual Task<IEnumerable<TEnity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return BaseEntitySet.GetAllAsync(cancellationToken);
        }

        public virtual void Add(TEnity entity) {
            BaseEntitySet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEnity> entities) {
            BaseEntitySet.AddRange(entities);
        }

        public virtual void Remove(TEnity entity) {
            BaseEntitySet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEnity> entities) {
            BaseEntitySet.RemoveRange(entities);
        }
    }
}