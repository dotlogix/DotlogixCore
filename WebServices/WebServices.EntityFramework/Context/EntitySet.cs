// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  EntitySet.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 21.11.2021 23:22
// LastEdited:  07.12.2021 10:50
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Context {
    /// <summary>
    /// An implementation of the <see cref="IEntitySet{TEntity}"/> interface for entity framework
    /// </summary>
    public class EntitySet<TEntity> : IEntitySet<TEntity> where TEntity : class, new() {
        private readonly DbSet<TEntity> _dbSet;


        /// <summary>
        /// Create a new instance of <see cref="EntitySet{TEntity}"/>
        /// </summary>
        public EntitySet(DbSet<TEntity> dbSet) {
            _dbSet = dbSet;
        }

        /// <inheritdoc />
        public virtual TEntity Add(TEntity entity) {
            return _dbSet.Add(entity).Entity;
        }

        /// <inheritdoc />
        public virtual ICollection<TEntity> AddRange(IEnumerable<TEntity> entities) {
            var entityCollection = entities.AsCollection();
            _dbSet.AddRange(entityCollection);
            return entityCollection;
		}

        /// <inheritdoc />
        public virtual TEntity Remove(TEntity entity)
        {
            return _dbSet.Remove(entity).Entity;
		}

        /// <inheritdoc />
        public virtual ICollection<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            var entityCollection = entities.AsCollection();
            _dbSet.RemoveRange(entityCollection);
            return entityCollection;
		}

        /// <inheritdoc />
        public IQueryable<TEntity> Query() {
            return _dbSet;
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Query(string sql, object[] parameters) {
            return _dbSet.FromSqlRaw(sql, parameters);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Query(FormattableString sql) {
            return _dbSet.FromSqlInterpolated(sql);
        }
    }
}