﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EfEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Architecture.Common.Options;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// An implementation of the <see cref="IEntityContext"/> interface for entity framework
    /// </summary>
    public class EfEntityContext : IEntityContext {
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
        public virtual IEntitySet<TEntity> UseSet<TEntity>() where TEntity : class
        {
	        var entityType = typeof(TEntity);
	        var dbSet = DbContext.Set<TEntity>();
	        if (entityType.IsAssignableTo<IGuid>()){
		        return typeof(EfGuidEntitySet<>)
		               .MakeGenericType(entityType)
		               .Instantiate<IEntitySet<TEntity>>(dbSet);
	        }

	        if (entityType.IsAssignableTo<IIdentity>()) {
		        return typeof(EfIdEntitySet<>)
		               .MakeGenericType(entityType)
		               .Instantiate<IEntitySet<TEntity>>(dbSet);
	        }

	        return new EfEntitySet<TEntity>(dbSet);
		}
    }
}
