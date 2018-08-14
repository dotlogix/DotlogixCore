// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EntitySetDecoratorBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public abstract class EntitySetDecoratorBase<TEnity> : IEntitySet<TEnity> where TEnity : ISimpleEntity {
        protected IEntitySet<TEnity> BaseEntitySet { get; }

        protected EntitySetDecoratorBase(IEntitySet<TEnity> baseEntitySet) {
            BaseEntitySet = baseEntitySet;
        }

        public virtual IQuery<TEnity> Query() {
            return BaseEntitySet.Query();
        }

        public virtual Task<TEnity> GetAsync(int id, CancellationToken cancellationToken = default) {
            return BaseEntitySet.GetAsync(id, cancellationToken);
        }

        public virtual Task<IEnumerable<TEnity>> GetRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default) {
            return BaseEntitySet.GetRangeAsync(ids, cancellationToken);
        }

        public virtual Task<TEnity> GetAsync(Guid guid, CancellationToken cancellationToken = default) {
            return BaseEntitySet.GetAsync(guid, cancellationToken);
        }

        public virtual Task<IEnumerable<TEnity>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default) {
            return BaseEntitySet.GetRangeAsync(guids, cancellationToken);
        }

        public virtual Task<IEnumerable<TEnity>> GetAllAsync(CancellationToken cancellationToken = default) {
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

        public void ReAttach(TEnity entity) {
            BaseEntitySet.ReAttach(entity);
        }

        public void ReAttachRange(IEnumerable<TEnity> entities) {
            BaseEntitySet.ReAttachRange(entities);
        }
    }
}
