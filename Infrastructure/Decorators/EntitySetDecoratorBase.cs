// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EntitySetDecoratorBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Architecture.Infrastructure.Queries.Queryable;
using DotLogix.Core;
#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    /// A decorator for <see cref="IEntitySet{TEntity}"/> to intercept requests
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntitySetDecoratorBase<TEntity> : EntitySetBase<TEntity>, IEntitySet<TEntity> where TEntity : ISimpleEntity {
        /// <summary>
        /// The internal base entity set
        /// </summary>
        protected IEntitySet<TEntity> BaseEntitySet { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EntitySetDecoratorBase{TEntity}"/>
        /// </summary>
        protected EntitySetDecoratorBase(IEntitySet<TEntity> baseEntitySet) {
            BaseEntitySet = baseEntitySet;
        }

        /// <inheritdoc />
        public override IQuery<TEntity> Query() {
            var query = BaseEntitySet.Query();
            return query;
        }

        /// <inheritdoc />
        public override void Add(TEntity entity) {
            BaseEntitySet.Add(entity);
        }

        /// <inheritdoc />
        public override void AddRange(IEnumerable<TEntity> entities) {
            BaseEntitySet.AddRange(entities);
        }

        /// <inheritdoc />
        public override void Remove(TEntity entity) {
            BaseEntitySet.Remove(entity);
        }

        /// <inheritdoc />
        public override void RemoveRange(IEnumerable<TEntity> entities) {
            BaseEntitySet.RemoveRange(entities);
        }

        /// <inheritdoc />
        public override void ReAttach(TEntity entity) {
            BaseEntitySet.ReAttach(entity);
        }

        /// <inheritdoc />
        public override void ReAttachRange(IEnumerable<TEntity> entities) {
            BaseEntitySet.ReAttachRange(entities);
        }
    }
}
