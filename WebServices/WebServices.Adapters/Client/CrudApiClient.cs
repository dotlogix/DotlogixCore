using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Common;
using DotLogix.Common.Features;
using DotLogix.WebServices.Adapters.Endpoints;
using DotLogix.WebServices.Adapters.Http;

namespace DotLogix.WebServices.Adapters.Client; 

public abstract class CrudApiClient<TResponse, TEnsure, TFilter>
    : CrudApiClient<TResponse, TEnsure, TEnsure, TEnsure, TFilter>
    where TResponse : IGuid
    where TEnsure : IGuid
{
    protected CrudApiClient(IHttpProvider httpProvider, IWebServiceEndpoint endpoint, string relativeUri)
        : base(httpProvider, endpoint, relativeUri)
    {
    }
}

public abstract class CrudApiClient<TResponse, TCreate, TEnsure, TPatch, TFilter>
    : ReadOnlyApiClient<TResponse, TFilter>, ICrudApiClient<TResponse, TCreate, TEnsure, TPatch, TFilter>
    where TResponse : IGuid
    where TCreate : IGuid
    where TEnsure : IGuid
    where TPatch : IGuid
{
    protected CrudApiClient(IHttpProvider httpProvider, IWebServiceEndpoint endpoint, string relativeUri)
        : base(httpProvider, endpoint, relativeUri)
    {
    }

    public virtual Task<TResponse> CreateAsync(TCreate request, CancellationToken cancellationToken = default)
    {
        request.EnsureGuid();
        return PostAsync<TResponse>(request, $"{request.Guid:D}", cancellationToken);
    }

    public virtual Task<ICollection<TResponse>> CreateRangeAsync(IEnumerable<TCreate> requests, CancellationToken cancellationToken = default)
    {
        return PostAsync<ICollection<TResponse>>(requests, cancellationToken: cancellationToken);
    }

    public virtual Task<TResponse> EnsureAsync(TEnsure request, CancellationToken cancellationToken = default)
    {
        request.EnsureGuid();
        return PutAsync<TResponse>(request, $"{request.Guid:D}", cancellationToken);
    }

    public virtual Task<ICollection<TResponse>> EnsureRangeAsync(IEnumerable<TEnsure> requests, CancellationToken cancellationToken = default)
    {
        return PutAsync<ICollection<TResponse>>(requests, cancellationToken: cancellationToken);
    }

    public virtual Task<TResponse> PatchAsync(TPatch request, CancellationToken cancellationToken = default)
    {
        return PatchAsync<TResponse>(request, $"{request.Guid:D}", cancellationToken);
    }

    public virtual Task<ICollection<TResponse>> PatchRangeAsync(IEnumerable<TPatch> requests, CancellationToken cancellationToken = default)
    {
        return PatchAsync<ICollection<TResponse>>(requests, cancellationToken: cancellationToken);
    }

    public virtual Task<bool> DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return DeleteAsync<bool>(null, $"{guid:D}", cancellationToken);
    }

    public virtual Task<int> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default)
    {
        return DeleteAsync<int>(guids, cancellationToken: cancellationToken);
    }
}