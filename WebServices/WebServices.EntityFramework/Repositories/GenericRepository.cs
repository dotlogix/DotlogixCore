using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Infrastructure.EntityFramework.Repositories;
using DotLogix.Infrastructure.Extensions;
using DotLogix.Infrastructure.Queries;
using DotLogix.WebServices.Core;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DotLogix.WebServices.EntityFramework.Repositories {
    [SuppressMessage("ReSharper", "EF1001")]
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    public class GenericRepository<TKey, TEntity> : EfRepositoryBase<TKey, TEntity>, IGenericRepository<TKey, TEntity> where TEntity : class, new() {
        protected new IWebServiceEntityContext EntityContext => (IWebServiceEntityContext)base.EntityContext;
        protected CacheOptions CacheOptions { get; set; } = CacheOptions.All;
        

        /// <inheritdoc />
        public GenericRepository(IWebServiceEntityContext entityContext) : base(entityContext) {
            
        }

        public override Task<TEntity> GetAsync(TKey key, CancellationToken cancellationToken = default) {
            return GetAsync(key, CacheOptions, cancellationToken);
        }

        public override Task<ICollection<TEntity>> GetRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default) {
            return GetRangeAsync(keys, CacheOptions, cancellationToken);
        }
        
        public virtual async Task<TEntity> GetAsync(TKey key, CacheOptions cacheOptions = CacheOptions.All, CancellationToken cancellationToken = default) {
            cacheOptions &= CacheOptions;
            
            if((cacheOptions & CacheOptions.Local) != 0 && TryGetLocal(key, out var entity)) {
                return entity;
            }

            if((cacheOptions & CacheOptions.Global) != 0 && TryGetGlobal(key, out entity, true)) {
                return entity;
            }
            
            return await base.GetAsync(key, cancellationToken);
        }

        public virtual async Task<ICollection<TEntity>> GetRangeAsync(IEnumerable<TKey> keys, CacheOptions cacheOptions = CacheOptions.All, CancellationToken cancellationToken = default) {
            cacheOptions &= CacheOptions;
            
            var resultEntities = new List<TEntity>();
            var remainingKeys = keys.AsReadOnlyCollection();
            if((cacheOptions & CacheOptions.Local) != 0 && TryGetLocal(remainingKeys, out var entities, out remainingKeys)) {
                if(entities.Count > 0) {
                    resultEntities.AddRange(entities);
                }
            }

            if((cacheOptions & CacheOptions.Global) != 0 && TryGetGlobal(remainingKeys, out entities, out remainingKeys, true)) {
                if(entities.Count > 0) {
                    resultEntities.AddRange(entities);
                }
            }

            if(remainingKeys.Count > 0) {
                var keyColumn = KeyProperty.Name;
                var dbEntities = await Query()
                                      .Where(e => remainingKeys.Contains(EF.Property<TKey>(e, keyColumn)))
                                                      .ToListAsync(cancellationToken);
                resultEntities.AddRange(dbEntities);
            }

            return resultEntities;
        }
        

        /// <inheritdoc />
        public override async Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) {
            return await Query().ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task<ICollection<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken = default) {
            return await Query()
                  .Where(filterExpression)
                  .ToListAsync(cancellationToken);
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
        public virtual Task<PaginationResult<TEntity>> GetPagedAsync(IQueryModifier<TEntity, TEntity> queryModifier, PaginationTerm pagination, CancellationToken cancellationToken = default) {
            return Query()
                  .Apply<TEntity>(EntityContext, queryModifier)
                  .ToPagedAsync(pagination, cancellationToken);
        }

        #region Helper
        protected virtual bool TryGetLocal(TKey key, out TEntity entity) {
            var stateManager = DbContext.GetDependencies().StateManager;
            var entry = stateManager.TryGetEntry(PrimaryKey, key.CreateArray<object>());
            if(entry == null) {
                entity = default;
                return false;
            }

            switch(entry.EntityState) {
                case EntityState.Detached:
                case EntityState.Deleted:
                    entity = default;
                    return true;
                case EntityState.Unchanged:
                case EntityState.Modified:
                case EntityState.Added:
                    entity = (TEntity)entry.Entity;
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual bool TryGetLocal(IEnumerable<TKey> keys, out IReadOnlyCollection<TEntity> entities, out IReadOnlyCollection<TKey> missingKeys) {
            var missing = new List<TKey>();
            var existing = new List<TEntity>();
            foreach(var key in keys) {
                if(TryGetLocal(key, out var entity) == false) {
                    missing.Add(key);
                } else if(entity != null) {
                    existing.Add(entity);
                }
            }

            entities = existing;
            missingKeys = missing;
            return missing.Count == 0;
        }

        protected virtual bool TryGetGlobal(TKey key, out TEntity entity, bool attach = false) {
            if(EntityContext.EntityCache.TryRetrieve(key, out entity) == false) {
                return false;
            }

            if(attach && entity != null) {
                entity = EntitySet.Attach(entity);
            }
            return true;
        }

        protected virtual bool TryGetGlobal(IEnumerable<TKey> keys, out IReadOnlyCollection<TEntity> entities, out IReadOnlyCollection<TKey> missingKeys, bool attach = false) {
            var missing = new List<TKey>();
            var existing = new List<TEntity>();
            foreach(var key in keys) {
                if(TryGetGlobal(key, out var entity) == false) {
                    missing.Add(key);
                } else if(entity != null) {
                    existing.Add(entity);
                }
            }

            missingKeys = missing;
            entities = attach
                           ? EntitySet.AttachRange(existing).AsReadOnlyCollection()
                           : existing;
            
            return missing.Count == 0;
        }
        #endregion
    }
}
