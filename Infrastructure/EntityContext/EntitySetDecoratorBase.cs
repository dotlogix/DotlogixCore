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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Architecture.Common.Entities;
using DotLogix.Architecture.Infrastructure.Queries;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public abstract class EntitySetDecoratorBase<TEntity> : EntitySetBase<TEntity>, IEntitySet<TEntity> where TEntity : ISimpleEntity {
        protected IEntitySet<TEntity> BaseEntitySet { get; }

        protected EntitySetDecoratorBase(IEntitySet<TEntity> baseEntitySet) {
            BaseEntitySet = baseEntitySet;
        }

        public override IQuery<TEntity> Query() {
            return BaseEntitySet.Query();
        }

        public override void Add(TEntity entity) {
            BaseEntitySet.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities) {
            BaseEntitySet.AddRange(entities);
        }

        public override void Remove(TEntity entity) {
            BaseEntitySet.Remove(entity);
        }

        public override void RemoveRange(IEnumerable<TEntity> entities) {
            BaseEntitySet.RemoveRange(entities);
        }

        public override void ReAttach(TEntity entity) {
            BaseEntitySet.ReAttach(entity);
        }

        public override void ReAttachRange(IEnumerable<TEntity> entities) {
            BaseEntitySet.ReAttachRange(entities);
        }
    }
}
