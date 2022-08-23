using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Mappers;
using DotLogix.WebServices.Core.Errors;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Database;
using DotLogix.WebServices.EntityFramework.Repositories;

namespace DotLogix.WebServices.AspCore.Services; 

public abstract class ReadOnlyDomainService<TEntity, TResponse> : IReadOnlyDomainService<TResponse>
    where TEntity : class, IGuid, new()
    where TResponse : class, IGuid, new() {
    private static IMapper<TEntity, TResponse> _mapper;
    protected IMapper<TEntity, TResponse> EntityMapper => _mapper ??= ConfigureEntityMapper();
    protected IRepository<Guid, TEntity> Repository { get; }

    public ReadOnlyDomainService(IRepository<Guid, TEntity> repository) {
        Repository = repository;
    }

    public virtual async Task<TResponse> GetAsync(Guid guid) {
        var entity = await Repository.GetAsync(guid);
        if (entity == null)
        {
            throw new FilterNotFoundException(guid) {
                ClrType = typeof(TResponse)
            };
        }
        return await ToModelAsync(entity);
    }

    public virtual async Task<ICollection<TResponse>> GetRangeAsync(IEnumerable<Guid> guids) {
        var entities = await Repository.GetRangeAsync(guids);
        return await ToModelsAsync(entities);
    }

    public virtual async Task<ICollection<TResponse>> GetAllAsync() {
        var entities = await Repository.GetAllAsync();
        return await ToModelsAsync(entities);
    }

    protected virtual Task<TResponse> ToModelAsync(TEntity entity) {
        return Task.FromResult(ToModel(entity));
    }

    protected virtual Task<ICollection<TResponse>> ToModelsAsync(IEnumerable<TEntity> entities) {
        var models = entities.Select(e => ToModel(e)).AsCollection();
        return Task.FromResult(models);
    }

    protected virtual TResponse ToModel(TEntity entity, TResponse model = null) {
        if(entity == null) {
            return null;
        }

        model ??= new TResponse();
        EntityMapper.Map(entity, model);
        return model;
    }

    protected virtual IMapper<TEntity, TResponse> ConfigureEntityMapper() {
        return MapBuilders.AutoMap<TEntity, TResponse>();
    }
}

public abstract class ReadOnlyDomainService<TEntity, TResponse, TFilter> : ReadOnlyDomainService<TEntity, TResponse>, IReadOnlyDomainService<TResponse, TFilter>
    where TEntity : class, IGuid, new()
    where TResponse : class, IGuid, new() {
    public ReadOnlyDomainService(IRepository<Guid, TEntity> repository) : base(repository) {
    }

    public virtual async Task<ICollection<TResponse>> GetFilteredAsync(TFilter filter) {
        var entities = filter is not null
            ? await Repository.WhereAsync(ToQueryModifier(filter))
            : await Repository.GetAllAsync();
        return await ToModelsAsync(entities);
    }

    public virtual Task<PaginationResult<TResponse>> GetPagedAsync(PaginationTerm paginationTerm) {
        return GetPagedAsync(default, paginationTerm);
    }

    public virtual async Task<PaginationResult<TResponse>> GetPagedAsync(TFilter filter, PaginationTerm paginationTerm) {
        var paginationResult = filter is not null
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