// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Attributes;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Collections;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// An implementation of the <see cref="IEntityContext"/> interface for entity framework
    /// </summary>
    public class EfEntityContext : IEntityContext {
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<Type, IEnumerable> ModifierDict { get; } = new Dictionary<Type, IEnumerable>();

        /// <summary>
        /// The internal <see cref="DbContext"/>
        /// </summary>
        public DbContext DbContext { get; }

        /// <inheritdoc />
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Create a new instance of <see cref="EfEntityContext"/>
        /// </summary>
        public EfEntityContext(DbContext dbContext) {
            DbContext = dbContext;
        }

        /// <inheritdoc />
        public virtual void Dispose() {
            DbContext.Dispose();
        }

        /// <inheritdoc />
        public virtual Task CompleteAsync() {
            return DbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public virtual IEntitySet<TEntity> UseSet<TEntity>() where TEntity : class, new() {
            var modifiers = (IEnumerable<Func<IEntitySet<TEntity>, IEntitySet<TEntity>>>)ModifierDict.GetOrAdd(typeof(TEntity), OnCreateModifiers<TEntity>);
            IEntitySet<TEntity> entitySet = new EfEntitySet<TEntity>(DbContext.Set<TEntity>());

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach(var modifier in modifiers) {
                entitySet = modifier.Invoke(entitySet);
            }

            return entitySet;
        }

        /// <summary>
        /// Creates entity set modifiers configured for this entity
        /// </summary>
        protected virtual ICollection<Func<IEntitySet<TEntity>, IEntitySet<TEntity>>> OnCreateModifiers<TEntity>() where TEntity : class, new() {
            var entityType = typeof(TEntity);
            var types = new List<Type>();
            types.Add(entityType);
            types.AddRange(entityType.GetTypesAssignableTo());

            var decoratorAttributes = types.SelectMany(t => t.GetCustomAttributes<EntitySetModifierAttribute>());

            return decoratorAttributes
                             .Distinct()
                             .OrderBy(d => d.Priority)
                             .Select(d => d.GetModifierFunc<TEntity>())
                             .ToList();
        }
    }
}
