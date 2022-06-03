#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Common;
using DotLogix.Common.Features;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Mappers;
using DotLogix.WebServices.Core.Errors;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Database;
using DotLogix.WebServices.EntityFramework.Repositories;
#endregion

namespace DotLogix.WebServices.AspCore.Services {
    public abstract class DomainService<TEntity, TResponse, TEnsure, TFilter> : DomainService<TEntity, TResponse, TEnsure, TEnsure, TEnsure, TFilter>
        where TEntity : class, IGuid, new()
        where TResponse : class, IGuid, new()
        where TEnsure : class, IGuid {
        protected DomainService(IRepository<Guid, TEntity> repository) : base(repository) {}

        protected override Task<TResponse> CreateEntityAsync(TEnsure requests, TEntity entity = default) {
            return EnsureEntityAsync(requests, entity);
        }

        protected override Task<TResponse> PatchEntityAsync(TEnsure requests, TEntity entity = default) {
            return EnsureEntityAsync(requests, entity);
        }
    }
    
    public abstract class DomainService<TEntity, TResponse, TEnsure> : DomainService<TEntity, TResponse, TEnsure, TEnsure, TEnsure>
        where TEntity : class, IGuid, new()
        where TResponse : class, IGuid, new()
        where TEnsure : class, IGuid {
        protected DomainService(IRepository<Guid, TEntity> repository) : base(repository) {}
        
        protected override Task<TResponse> CreateEntityAsync(TEnsure requests, TEntity entity = default) {
            return EnsureEntityAsync(requests, entity);
        }

        protected override Task<TResponse> PatchEntityAsync(TEnsure requests, TEntity entity = default) {
            return EnsureEntityAsync(requests, entity);
        }
    }

    public abstract class DomainService<TEntity, TResponse, TCreate, TEnsure, TPatch> : ReadOnlyDomainService<TEntity, TResponse>, IDomainService<TResponse, TCreate, TEnsure, TPatch>
        where TEntity : class, IGuid, new()
        where TResponse : class, IGuid, new()
        where TCreate : class, IGuid
        where TEnsure : class, IGuid
        where TPatch : class, IGuid {
        private static IMapper<TCreate, TEntity> _createMapper;
        private static IMapper<TEnsure, TEntity> _ensureMapper;
        private static IMapper<TPatch, TEntity> _patchMapper;
        protected IMapper<TCreate, TEntity> CreateMapper => _createMapper ??= ConfigureCreateMapper();
        protected IMapper<TEnsure, TEntity> EnsureMapper => _ensureMapper ??= ConfigureEnsureMapper();
        protected IMapper<TPatch, TEntity> PatchMapper => _patchMapper ??= ConfigurePatchMapper();


        protected DomainService(IRepository<Guid, TEntity> repository)
            : base(repository) {
        }

        #region Remove
        public virtual async Task<bool> RemoveAsync(Guid guid) {
            var entity = await Repository.GetAsync(guid);
            if(entity == null) {
                return false;
            }

            await RemoveEntityAsync(entity);
            return true;
        }

        public virtual async Task<int> RemoveRangeAsync(IEnumerable<Guid> guids) {
            var entities = await Repository.GetRangeAsync(guids);
            var count = 0;
            foreach(var entity in entities) {
                if(await RemoveEntityAsync(entity)) {
                    count++;
                }
            }
            return count;
        }
        
        public virtual Task<bool> RemoveEntityAsync(TEntity entity) {
            Repository.Remove(entity);
            return Task.FromResult(true);
        }
        #endregion

        #region Create
        public virtual async Task<TResponse> CreateAsync(TCreate request) {
            if(request.Guid != Guid.Empty)
            {
                var entity = await Repository.GetAsync(request.Guid);
                if (entity != null)
                {
                    // creation of an already existing entity is not allowed
                    throw new ConflictApiException {
                        ExistingObject = await ToModelAsync(entity),
                        ConflictObject = request
                    };
                }
            }

            request.EnsureGuid();
            return await CreateEntityAsync(request);
        }

        public virtual async Task<ICollection<TResponse>> CreateRangeAsync(IEnumerable<TCreate> requests) {
            requests = requests.AsCollection();
            var keys = requests.GetUniqueKeys();
            var entities = await Repository.GetRangeAsync(keys);
            var diff = requests.Diff(r => r.EnsureGuid(), entities, e => e.Guid);

            if (diff.Intersect.Count > 0)
            {
                // creation of already existing entities is not allowed
                var existingEntities = diff.Intersect.Select(i => i.Right);
                var conflictRequests = diff.Intersect.Select(i => i.Left).ToList();

                throw new ConflictApiException {
                    ExistingObject = await ToModelsAsync(existingEntities),
                    ExistingClrType = typeof(ICollection<TResponse>),
                    ConflictObject = conflictRequests,
                    ConflictClrType = typeof(ICollection<TCreate>),
                };
            }

            var models = new List<TResponse>(diff.LeftOnly.Count);
            foreach (var request in diff.LeftOnly)
            {
                var model = await CreateEntityAsync(request);
                if (model != null)
                {
                    models.Add(model);
                }
            }
            return models;
        }
        
        protected virtual async Task<TResponse> CreateEntityAsync(TCreate requests, TEntity entity = null) {
            if(entity == null) {
                entity = CreateMapper.Map(requests);
                Repository.Add(entity);
            } else {
                CreateMapper.Map(requests, entity);
            }

            return await ToModelAsync(entity);
        }

        #endregion

        #region Ensure
        public virtual async Task<TResponse> EnsureAsync(TEnsure requests) {
            TEntity entity = null;
            if(requests.Guid != Guid.Empty)
            {
                entity = await Repository.GetAsync(requests.Guid);
            }

            requests.EnsureGuid();
            return await EnsureEntityAsync(requests, entity);
        }

        public virtual async Task<ICollection<TResponse>> EnsureRangeAsync(IEnumerable<TEnsure> requests) {
            requests = requests.AsCollection();
            var entities = await Repository.GetRangeAsync(requests.GetUniqueKeys());
            var diff = requests.Diff(r => r.EnsureGuid(), entities, e => e.Guid);
            
            var models = new List<TResponse>(diff.LeftOnly.Count + diff.Intersect.Count);
            foreach (var request in diff.LeftOnly)
            {
                var model = await EnsureEntityAsync(request);
                if(model != null)
                {
                    models.Add(model);
                }
            }

            foreach (var (request, entity) in diff.Intersect)
            {
                var model = await EnsureEntityAsync(request, entity);
                if(model != null)
                {
                    models.Add(model);
                }
            }

            return models;
        }
        protected virtual async Task<TResponse> EnsureEntityAsync(TEnsure requests, TEntity entity = null) {
            if(entity == null) {
                entity = EnsureMapper.Map(requests);
                Repository.Add(entity);
            } else {
                EnsureMapper.Map(requests, entity);
            }

            return await ToModelAsync(entity);
        }

        #endregion

        #region Patch
        public virtual async Task<TResponse> PatchAsync(TPatch request) {
            var entity = await Repository.GetAsync(request.Guid);
            if (entity == null)
            {
                throw new FilterNotFoundException(request.Guid) {
                    ClrType = typeof(TResponse)
                };
            }
            request.EnsureGuid();
            return await PatchEntityAsync(request, entity);
        }

        public virtual async Task<ICollection<TResponse>> PatchRangeAsync(IEnumerable<TPatch> requests) {
            requests = requests.AsCollection();
            var entities = await Repository.GetRangeAsync(requests.GetUniqueKeys());
            var diff = requests.Diff(r => r.EnsureGuid(), entities, e => e.Guid);
            if (diff.LeftOnly.Count > 0) {
                var missingGuids = diff.LeftOnly.GetUniqueKeys();
                throw new FilterNotFoundException(missingGuids) {
                    ClrType = typeof(ICollection<TResponse>),
                    FilterClrType = typeof(ICollection<Guid>)
                };
            }

            var models = new List<TResponse>(diff.Intersect.Count);
            foreach (var (request, entity) in diff.Intersect)
            {
                var model = await PatchEntityAsync(request, entity);
                if(model != null)
                {
                    models.Add(model);
                }
            }

            return models;
        }
        
        protected virtual async Task<TResponse> PatchEntityAsync(TPatch requests, TEntity entity = null) {
            if(entity == null) {
                return null;
            }

            PatchMapper.Map(requests, entity);
            return await ToModelAsync(entity);
        }
        #endregion

        #region Mappers
        protected virtual IMapper<TCreate, TEntity> ConfigureCreateMapper() {
            return OptionalMapBuilders.AutoMap<TCreate, TEntity>(ignoreUndefinedOptional: true);
        }

        protected virtual IMapper<TEnsure, TEntity> ConfigureEnsureMapper() {
            return OptionalMapBuilders.AutoMap<TEnsure, TEntity>(ignoreUndefinedOptional: false);
        }

        protected virtual IMapper<TPatch, TEntity> ConfigurePatchMapper() {
            return OptionalMapBuilders.AutoMap<TPatch, TEntity>(ignoreUndefinedOptional: true);
        }
        #endregion
    }
    
    
    public abstract class DomainService<TEntity, TResponse, TCreate, TEnsure, TPatch, TFilter> : DomainService<TEntity, TResponse, TCreate, TEnsure, TPatch>, IDomainService<TResponse, TCreate, TEnsure, TPatch, TFilter>
        where TEntity : class, IGuid, new()
        where TResponse : class, IGuid, new()
        where TCreate : class, IGuid
        where TEnsure : class, IGuid
        where TPatch : class, IGuid {
        protected DomainService(IRepository<Guid, TEntity> repository) : base(repository) {
        }

        public virtual async Task<ICollection<TResponse>> GetFilteredAsync(TFilter filter) {
            var entities = filter != null
                               ? await Repository.WhereAsync(ToQueryModifier(filter))
                               : await Repository.GetAllAsync();
            return await ToModelsAsync(entities);
        }

        public Task<PaginationResult<TResponse>> GetPagedAsync(PaginationTerm paginationTerm) {
            return GetPagedAsync(default, paginationTerm);
        }

        public virtual async Task<PaginationResult<TResponse>> GetPagedAsync(TFilter filter, PaginationTerm paginationTerm) {
            var paginationResult = filter != null
                                       ? await Repository.GetPagedAsync(ToQueryModifier(filter), paginationTerm)
                                       : await Repository.GetPagedAsync(paginationTerm);

            return new PaginationResult<TResponse> {
                Page = paginationResult.Page,
                TotalCount = paginationResult.TotalCount,
                PageSize = paginationResult.PageSize,
                Values = await ToModelsAsync(paginationResult.Values),
            };
        }
        protected abstract IEntityQuery<TEntity, TEntity> ToQueryModifier(TFilter filter);
    }
}
