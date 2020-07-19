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
using System.Threading.Tasks;
using DotLogix.Architecture.Common.Options;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Architecture.Infrastructure.Queries;
using DotLogix.Core.Extensions;

#endregion

namespace DotLogix.Architecture.Infrastructure.Decorators {
    /// <summary>
    ///     A entity set decorator to disable deletion of entities instead manage existence with the is active flag
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class InsertOnlyEntitySetDecorator<TEntity> : EntitySetDecoratorBase<TEntity> where TEntity : class, IInsertOnly, new() {
        /// <summary>
        ///     Creates a new instance of <see cref="InsertOnlyEntitySetDecorator{TEntity}" />
        /// </summary>
        public InsertOnlyEntitySetDecorator(IEntitySet<TEntity> baseEntitySet) : base(baseEntitySet) { }

        /// <inheritdoc />
        public override ValueTask<TEntity> AddAsync(TEntity entity) {
            entity.IsActive = true;
            return BaseEntitySet.AddAsync(entity);
        }

        /// <inheritdoc />
        public override ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities) {
            var collection = entities.AsCollection();
            foreach(var entity in collection)
                entity.IsActive = true;
            return BaseEntitySet.AddRangeAsync(collection);
        }

        /// <summary>
        ///     Mark a single entity as deleted by setting its IsActive flag to false
        /// </summary>
        public override ValueTask<TEntity> RemoveAsync(TEntity entity) {
            entity.IsActive = false;
            return new ValueTask<TEntity>(entity);
        }

        /// <summary>
        ///     Mark a range of entities as deleted by setting their IsActive flag to false
        /// </summary>
        public override ValueTask<IEnumerable<TEntity>> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
	        var collection = entities.AsCollection();
			foreach(var entity in collection)
                entity.IsActive = true;

			return new ValueTask<IEnumerable<TEntity>>(collection);
        }

        /// <inheritdoc />
        public override IQuery<TEntity> Query() {
            return BaseEntitySet.Query().Where(e => e.IsActive);
        }
    }
}
