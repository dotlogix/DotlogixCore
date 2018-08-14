// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EntitySetBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public abstract class EntitySetBase<TEntity> : IEntitySet<TEntity> where TEntity : ISimpleEntity {
        public Task<TEntity> GetAsync(int id, CancellationToken cancellationToken = default) {
            return Query().Where(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
            return Query().Where(e => ids.Contains(e.Id)).ToEnumerableAsync(cancellationToken);
        }

        public Task<TEntity> GetAsync(Guid guid, CancellationToken cancellationToken = default) {
            return Query().Where(e => e.Guid == guid).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<IEnumerable<TEntity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default) {
            return Query().Where(e => guids.Contains(e.Guid)).ToEnumerableAsync(cancellationToken);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            return Query().ToEnumerableAsync(cancellationToken);
        }

        public abstract void Add(TEntity entity);
        public abstract void AddRange(IEnumerable<TEntity> entities);
        public abstract void Remove(TEntity entity);
        public abstract void RemoveRange(IEnumerable<TEntity> entities);
        public abstract void ReAttach(TEntity entity);
        public abstract void ReAttachRange(IEnumerable<TEntity> entities);
        public abstract IQuery<TEntity> Query();

        public Task<IEnumerable<TEntity>> FilterAllAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return Query().Where(filterExpression).ToEnumerableAsync(cancellationToken);
        }
    }
}
