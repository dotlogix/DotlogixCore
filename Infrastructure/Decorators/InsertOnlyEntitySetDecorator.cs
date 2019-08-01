// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyEntitySetDecorator.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries;
#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    ///     A entity set decorator to disable deletion of entities instead manage existence with the is active flag
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class InsertOnlyEntitySetDecorator<TEntity> : EntitySetDecoratorBase<TEntity> where TEntity : class, IInsertOnlyEntity {
        /// <summary>
        ///     Creates a new instance of <see cref="InsertOnlyEntitySetDecorator{TEntity}" />
        /// </summary>
        public InsertOnlyEntitySetDecorator(IEntitySet<TEntity> baseEntitySet) : base(baseEntitySet) { }

        /// <inheritdoc />
        public override void Add(TEntity entity) {
            entity.IsActive = true;
            BaseEntitySet.Add(entity);
        }

        /// <inheritdoc />
        public override void AddRange(IEnumerable<TEntity> entities) {
            var list = entities.ToList();
            foreach(var entity in list)
                entity.IsActive = true;
            BaseEntitySet.AddRange(list);
        }

        /// <summary>
        ///     Mark a single entity as deleted by setting its IsActive flag to false
        /// </summary>
        public override void Remove(TEntity entity) {
            entity.IsActive = false;
        }

        /// <summary>
        ///     Mark a range of entities as deleted by setting their IsActive flag to false
        /// </summary>
        public override void RemoveRange(IEnumerable<TEntity> entities) {
            foreach(var entity in entities)
                entity.IsActive = true;
        }

        /// <inheritdoc />
        public override IQuery<TEntity> Query() {
            return BaseEntitySet.Query().Where(e => e.IsActive);
        }
    }
}
