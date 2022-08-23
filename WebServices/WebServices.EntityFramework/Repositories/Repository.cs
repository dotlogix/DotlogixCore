// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepositoryBase.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Expressions;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Database;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
#endregion

namespace DotLogix.WebServices.EntityFramework.Repositories; 

/// <summary>
///     A generic ef repository providing crud functionality
/// </summary>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class {
    private IEntitySet<TEntity> _entitySet;

    /// <summary>
    ///     Gets the entity that maps the given entity class.
    /// </summary>
    protected IEntityType EntityType { get; }
        
    /// <summary>
    ///     Gets the entity clr type that maps the given entity class.
    /// </summary>
    protected Type EntityClrType => EntityType.ClrType;

    /// <summary>
    ///     The hooks for this instance
    /// </summary>
    protected IEntityEventHandler EventHandler { get; }

    /// <summary>
    ///     The current entity context
    /// </summary>
    protected IEntityContext EntityContext { get; }

    /// <summary>
    ///     The undecorated and unmodified entity set
    /// </summary>
    public IEntitySet<TEntity> EntitySet => _entitySet ??= EntityContext.Set<TEntity>();

    /// <summary>
    ///     Creates a new instance of <see cref="Repository{TEntity}" />
    /// </summary>
    protected Repository(IEntityContext entityContext) {
        EntityType = entityContext.TypeManager.GetEntityType<TEntity>();
        EventHandler = entityContext.EventManager.GetHandler(EntityType.ClrType);
        EntityContext = entityContext;
    }

    /// <inheritdoc />
    public virtual async Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await Query().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
        return await Query().CountAsync(filterExpression, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(IEntityQuery<TEntity, TEntity> filter, CancellationToken cancellationToken = default) {
        return await Query(filter).CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
        return await Query().AnyAsync(filterExpression, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> AnyAsync(IEntityQuery<TEntity, TEntity> filter, CancellationToken cancellationToken = default) {
        return await Query(filter).AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<ICollection<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
        return await Query().Where(filterExpression).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<ICollection<TEntity>> WhereAsync(IEntityQuery<TEntity, TEntity> filter, CancellationToken cancellationToken = default) {
        return await Query(filter).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<PaginationResult<TEntity>> GetPagedAsync(PaginationTerm pagination, CancellationToken cancellationToken = default) {
        return Query()
           .ToPagedAsync(pagination, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<PaginationResult<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> filterExpression, PaginationTerm pagination, CancellationToken cancellationToken = default) {
        return Query()
           .Where(filterExpression)
           .ToPagedAsync(pagination, cancellationToken);
    }
        
    /// <inheritdoc />
    public virtual Task<PaginationResult<TEntity>> GetPagedAsync(IEntityQuery<TEntity, TEntity> queryModifier, PaginationTerm pagination, CancellationToken cancellationToken = default) {
        return Query()
           .Apply<TEntity>(EntityContext, queryModifier)
           .ToPagedAsync(pagination, cancellationToken);
    }

    /// <inheritdoc />
    public virtual TEntity Add(TEntity entity) {
        return EntitySet.Add(entity);
    }

    /// <inheritdoc />
    public virtual ICollection<TEntity> AddRange(IEnumerable<TEntity> entities) {
        return EntitySet.AddRange(entities);
    }

    /// <inheritdoc />
    public virtual TEntity Remove(TEntity entity) {
        return EntitySet.Remove(entity);
    }

    /// <inheritdoc />
    public virtual ICollection<TEntity> RemoveRange(IEnumerable<TEntity> entities) {
        return EntitySet.RemoveRange(entities);
    }

    /// <summary>
    ///     Create a linq query to allow more advanced requests to the entity set
    /// </summary>
    protected virtual IQueryable<TEntity> Query() {
        return EntitySet.Query();
    }

    /// <summary>
    ///     Create a linq query to allow more advanced requests to the entity set
    /// </summary>
    protected virtual IQueryable<TEntity> Query(params IEntityQuery<TEntity, TEntity>[] filters) {
        return Query().Apply(EntityContext, filters);
    }
}

/// <summary>
///     A generic ef repository providing crud functionality
/// </summary>
public class Repository<TKey, TEntity> : Repository<TEntity>, IRepository<TKey, TEntity> where TEntity : class {
    /// <summary>
    ///  Gets the allowed cache options for this repository.
    /// </summary>
    protected CacheOptions CacheOptions { get; set; } = CacheOptions.All;
    /// <summary>
    ///     Gets primary key for this entity type. Returns <see langword="null" /> if no primary key is defined.
    /// </summary>
    protected IKey PrimaryKey { get; }
    /// <summary>
    ///     Gets the property that make up the key.
    /// </summary>
    protected IReadOnlyProperty KeyProperty => PrimaryKey.Properties.Single();

    /// <inheritdoc />
    public Repository(IEntityContext entityContext) : base(entityContext) {
        PrimaryKey = EntityType.FindPrimaryKey();
        ValidatePrimaryKey();
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> GetAsync(TKey key, CacheOptions cacheOptions = CacheOptions.All, CancellationToken cancellationToken = default) {
        cacheOptions &= CacheOptions;
            
        if((cacheOptions & CacheOptions.Local) != 0 && TryGetLocal(PrimaryKey, key, out var entity)) {
            return entity;
        }

        if((cacheOptions & CacheOptions.Global) != 0 && TryGetGlobal(key, out entity, true)) {
            return entity;
        }

        var (entityLambda, entityParameter) = Lambdas.Parameter<TEntity>();
        var condition = entityLambda
           .Property<TEntity, TKey>(KeyProperty)
           .IsEqualTo(key)
           .ToLambda<TEntity, bool>(entityParameter);
            
        return await Query()
           .Where(condition)
           .SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<ICollection<TEntity>> GetRangeAsync(IEnumerable<TKey> keys, CacheOptions cacheOptions = CacheOptions.All, CancellationToken cancellationToken = default) {
        cacheOptions &= CacheOptions;
            
        var resultEntities = new List<TEntity>();
        var remainingKeys = keys.AsReadOnlyCollection();
        if((cacheOptions & CacheOptions.Local) != 0 && TryGetLocal(PrimaryKey, remainingKeys, out var entities, out remainingKeys)) {
            resultEntities.AddRange(entities);
        }

        if((cacheOptions & CacheOptions.Global) != 0 && TryGetGlobal(remainingKeys, out entities, out remainingKeys, true)) {
            resultEntities.AddRange(entities);
        }

        if(remainingKeys.Count > 0) {
            var term = remainingKeys.ToManyTerm();
            var (entityLambda, entityParameter) = Lambdas.Parameter<TEntity>();
            var property = entityLambda.Property<TEntity, TKey>(KeyProperty);
            var condition = Lambdas
               .CallStatic(TermExtensions.Matches, Lambdas.Constant(term), property)
               .ToLambda<TEntity, bool>(entityParameter);
                
            var dbEntities = await Query()
               .Where(condition)
               .ToListAsync(cancellationToken);
            resultEntities.AddRange(dbEntities);
        }

        return resultEntities;
    }

    #region Helper
    protected virtual bool TryGetLocal(IKey dbKey, TKey key, out TEntity entity) {
        var entry = EntityContext.StateManager.GetEntry<TEntity>(dbKey, key);
        if(entry == null) {
            entity = default;
            return false;
        }

        switch(entry.State) {
            case EntityState.Detached:
            case EntityState.Deleted:
                entity = default;
                return true;
            case EntityState.Unchanged:
            case EntityState.Modified:
            case EntityState.Added:
                entity = entry.Entity;
                return true;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected virtual bool TryGetLocal(IKey dbKey, IEnumerable<TKey> keys, out IReadOnlyCollection<TEntity> entities, out IReadOnlyCollection<TKey> missingKeys) {
        var missing = new List<TKey>();
        var existing = new List<TEntity>();
        foreach(var key in keys) {
            if(TryGetLocal(dbKey, key, out var entity) == false) {
                missing.Add(key);
            } else if(entity is not null) {
                existing.Add(entity);
            }
        }

        entities = existing;
        missingKeys = missing;
        return existing.Count > 0;
    }

    protected virtual bool TryGetGlobal(TKey key, out TEntity entity, bool attach = false) {
        if(EntityContext.EntityCache.TryRetrieve(key, out entity) == false) {
            return false;
        }

        if(attach && entity is not null) {
            EntityContext.StateManager.Attach(entity);
        }
        return true;
    }

    protected virtual bool TryGetGlobal(IEnumerable<TKey> keys, out IReadOnlyCollection<TEntity> entities, out IReadOnlyCollection<TKey> missingKeys, bool attach = false) {
        var missing = new List<TKey>();
        var existing = new List<TEntity>();
        foreach(var key in keys) {
            if(TryGetGlobal(key, out var entity) == false) {
                missing.Add(key);
            } else if(entity is not null) {
                existing.Add(entity);
            }
        }

        missingKeys = missing;
        entities = existing;
            
        if(attach)
            EntityContext.StateManager.Attach(existing);
            
        return existing.Count > 0;
    }
    #endregion

    private void ValidatePrimaryKey() {
        if(PrimaryKey.Properties.Count != 1) {
            var message = $"Expected a single property of type {typeof(TKey)} as primary key, but got properties " +
                string.Join(", ", PrimaryKey.Properties.Select(e => e.ClrType.Name));
            throw new ArgumentException(message);
        }

        var primaryKeyProperty = PrimaryKey.Properties[0];
        if(primaryKeyProperty.ClrType != typeof(TKey)) {
            throw new ArgumentException($"Expected a single property of type {typeof(TKey)} as primary key, but got property {primaryKeyProperty.Name} with type {primaryKeyProperty.ClrType}");
        }
    }
}